import { AfterViewInit, Component, inject, OnInit, ViewChild, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { Sidebar } from '../../../shared/components/sidebar/sidebar';
import { WorkoutService } from '../workout.service';
import { Workout, ExerciseType } from '../models/workout.models';
import { WorkoutForm } from '../workout-form/workout-form';
import { MATERIAL_MODULES } from '../../../shared/material';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

const EXERCISE_TYPES: ExerciseType[] = ['Cardio', 'Strength', 'Flexibility', 'Sport', 'Yoga'];

@Component({
  selector: 'app-workout-list',
  imports: [Sidebar, DatePipe, ...MATERIAL_MODULES, ReactiveFormsModule],
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
  exerciseTypes = EXERCISE_TYPES;
  filterControl = new FormControl<ExerciseType | 'All'>('All');

  displayedColumns = ['exerciseType', 'performedAt', 'durationMinutes', 'caloriesBurned', 'intensity', 'fatigue', 'notes', 'actions'];

  ngOnInit(): void {
    this.loadWorkouts();

    this.dataSource.filterPredicate = (workout: Workout, filter: string) => { return filter === 'All' || workout.exerciseType === filter; };
    this.filterControl.valueChanges.subscribe((value) => {
      this.dataSource.filter = value ?? 'All';
    });

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
        this.dataSource.filter = this.filterControl.value ?? 'All';
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

  get hasFilteredWorkouts(): boolean {
  return this.dataSource.filteredData.length > 0;
}
}