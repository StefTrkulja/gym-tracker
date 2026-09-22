import { Component, inject, OnInit, signal } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Sidebar } from '../../../shared/components/sidebar/sidebar';
import { ProgressService } from '../../progress/progress.service';
import { WeeklyStats } from '../../progress/models/progress.models';
import { ChartConfiguration, ChartData, Chart, BarController, BarElement, CategoryScale, LinearScale, Legend, Tooltip } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';

Chart.register(BarController, BarElement, CategoryScale, LinearScale, Legend, Tooltip);

@Component({
  selector: 'app-dashboard',
  imports: [Sidebar, MatIconModule, MatButtonModule, MatCardModule, BaseChartDirective],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit {
  private progressService = inject(ProgressService);
  private snackBar = inject(MatSnackBar);

  weeks = signal<WeeklyStats[]>([]);
  isLoading = signal<boolean>(false);

  chartData: ChartData<'bar'> = { labels: [], datasets: [] };
 chartOptions: ChartConfiguration<'bar'>['options'] = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: { legend: { display: true, position: 'top' } },
  scales: {
    y: {
      type: 'linear',
      position: 'left',
      beginAtZero: true,
      title: { display: true, text: 'Minutes' },
    },
    y1: {
      type: 'linear',
      position: 'right',
      beginAtZero: true,
      title: { display: true, text: 'Calories' },
      grid: { drawOnChartArea: false },
    },
  },
};
  currentDate = new Date();

  private readonly monthNames = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December',
  ];

  get currentMonthLabel(): string {
    return `${this.monthNames[this.currentDate.getMonth()]} ${this.currentDate.getFullYear()}`;
  }

get monthSummary() {
  const weeks = this.weeks();
  const totalWorkouts = weeks.reduce((sum, w) => sum + w.workoutCount, 0);
  const totalMinutes = weeks.reduce((sum, w) => sum + w.totalDurationMinutes, 0);
  const totalCalories = weeks.reduce((sum, w) => sum + w.totalCaloriesBurned, 0);
  return { totalWorkouts, totalMinutes, totalCalories };
}
  ngOnInit(): void {
    this.loadProgress();
  }

  loadProgress(): void {
    this.isLoading.set(true);
    const year = this.currentDate.getFullYear();
    const month = this.currentDate.getMonth() + 1;

    this.progressService.getMonthlyProgress(year, month).subscribe({
      next: (data) => {
        this.weeks.set(data.weeks);
        this.updateChart(data.weeks);
        this.isLoading.set(false);
        console.log(data.weeks)
      },
      error: () => {
        this.isLoading.set(false);
        this.snackBar.open('Failed to load progress data.', 'OK', { duration: 4000 });
      },
    });
  }

private updateChart(weeks: WeeklyStats[]): void {
  this.chartData = {
    labels: weeks.map(w => `Week ${w.weekIndex}`),
    datasets: [
      {
        data: weeks.map(w => w.totalDurationMinutes),
        label: 'Duration (min)',
        backgroundColor: '#7c8fff',
        borderRadius: 6,
        yAxisID: 'y',
      },
      {
        data: weeks.map(w => w.totalCaloriesBurned),
        label: 'Calories',
        backgroundColor: '#ffa07a',
        borderRadius: 6,
        yAxisID: 'y1',
      },
    ],
  };
}

  previousMonth(): void {
    this.currentDate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth() - 1, 1);
    this.loadProgress();
  }

  nextMonth(): void {
    this.currentDate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth() + 1, 1);
    this.loadProgress();
  }

  formatDateRange(start: string, end: string): string {
    const startDate = new Date(start);
    const endDate = new Date(end);
    const options: Intl.DateTimeFormatOptions = { month: 'short', day: 'numeric' };
    return `${startDate.toLocaleDateString('en-US', options)} – ${endDate.toLocaleDateString('en-US', options)}`;
  }
}