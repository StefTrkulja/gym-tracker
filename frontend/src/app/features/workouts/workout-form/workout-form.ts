import { Component, inject } from '@angular/core';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSliderModule } from '@angular/material/slider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { WorkoutService } from '../workout.service';
import { ExerciseType, Workout } from '../models/workout.models';

const EXERCISE_TYPES: ExerciseType[] = ['Cardio', 'Strength', 'Flexibility', 'Sport', 'Yoga'];

@Component({
  selector: 'app-workout-form',
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatSliderModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './workout-form.html',
  styleUrl: './workout-form.scss',
})
export class WorkoutForm {
  private workoutsService = inject(WorkoutService);
  private snackBar = inject(MatSnackBar);
  private dialogRef = inject(MatDialogRef<WorkoutForm>);
  data = inject<{ workout?: Workout }>(MAT_DIALOG_DATA, { optional: true });

  exerciseTypes = EXERCISE_TYPES;
  isLoading = false;
  isEditMode = !!this.data?.workout;

  form = new FormGroup({
    exerciseType: new FormControl<ExerciseType>('Cardio', [Validators.required]),
    durationMinutes: new FormControl<number>(30, [Validators.required, Validators.min(1), Validators.max(1440)]),
    caloriesBurned: new FormControl<number>(200, [Validators.required, Validators.min(0), Validators.max(20000)]),
    intensity: new FormControl<number>(5, [Validators.required, Validators.min(1), Validators.max(10)]),
    fatigue: new FormControl<number>(5, [Validators.required, Validators.min(1), Validators.max(10)]),
    performedAt: new FormControl<string>(this.toLocalDateTimeString(new Date()), [Validators.required]),
    notes: new FormControl<string>(''),
  });

  constructor() {
    if (this.data?.workout) {
      const w = this.data.workout;
      this.form.patchValue({
        exerciseType: w.exerciseType,
        durationMinutes: w.durationMinutes,
        caloriesBurned: w.caloriesBurned,
        intensity: w.intensity,
        fatigue: w.fatigue,
        performedAt: this.toLocalDateTimeString(new Date(w.performedAt)),
        notes: w.notes ?? '',
      });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const value = this.form.getRawValue();

    const request = {
      exerciseType: value.exerciseType!,
      durationMinutes: value.durationMinutes!,
      caloriesBurned: value.caloriesBurned!,
      intensity: value.intensity!,
      fatigue: value.fatigue!,
      notes: value.notes || null,
      performedAt: new Date(value.performedAt!).toISOString(),
    };

    const request$ = this.isEditMode
      ? this.workoutsService.update(this.data!.workout!.id, request)
      : this.workoutsService.create(request);

    request$.subscribe({
      next: () => {
        this.isLoading = false;
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.isLoading = false;
        const message = err.error?.error ?? 'Something went wrong. Please try again.';
        this.snackBar.open(message, 'OK', { duration: 5000 });
      },
    });
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }

  private toLocalDateTimeString(date: Date): string {
    const pad = (n: number) => n.toString().padStart(2, '0');
    return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}`;
  }
}