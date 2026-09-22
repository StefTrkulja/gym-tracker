import { AfterViewInit, Component, inject, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Sidebar } from '../../../shared/components/sidebar/sidebar';
import { WorkoutService } from '../workout.service';
import { Workout } from '../models/workout.models';
import { WorkoutForm } from '../workout-form/workout-form';
import { signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-workout-list',
  imports: [Sidebar, MatTableModule, MatButtonModule, MatIconModule, MatChipsModule, DatePipe, MatTooltipModule, MatPaginatorModule],
  templateUrl: './workout-list.html',
  styleUrl: './workout-list.scss',
})
export class WorkoutList implements OnInit, AfterViewInit {
  private workoutsService = inject(WorkoutService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  dataSource = new MatTableDataSource<Workout>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  isLoading = signal<boolean>(false);

  displayedColumns = ['exerciseType', 'performedAt', 'durationMinutes', 'caloriesBurned', 'intensity', 'fatigue', 'notes', 'actions'];

  ngOnInit(): void {
    this.loadWorkouts();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  get hasWorkouts(): boolean {
    return this.dataSource.data.length > 0;
  }

  loadWorkouts(): void {
    this.isLoading.set(true);
    this.workoutsService.getAll().subscribe({
      next: (data) => {
        this.dataSource.data = data;
        this.isLoading.set(false);
        console.log(data)
        console.log("Test")
      },
      error: () => {
        console.log("Testic")
        this.isLoading.set(false);
        this.snackBar.open('Failed to load workouts.', 'OK', { duration: 4000 });
      },
    });
  }

  openAddDialog(): void {
    const dialogRef = this.dialog.open(WorkoutForm, { width: '480px' });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadWorkouts();
      }
    });
  }

  openEditDialog(workout: Workout): void {
    const dialogRef = this.dialog.open(WorkoutForm, {
      width: '480px',
      data: { workout },
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadWorkouts();
      }
    });
  }

  deleteWorkout(workout: Workout): void {
    if (!confirm('Delete this workout?')) return;

    this.workoutsService.delete(workout.id).subscribe({
      next: () => {
        this.snackBar.open('Workout deleted.', 'OK', { duration: 3000 });
        this.loadWorkouts();
      },
      error: () => {
        this.snackBar.open('Failed to delete workout.', 'OK', { duration: 4000 });
      },
    });
  }
}