export type ExerciseType = 'Cardio' | 'Strength' | 'Flexibility' | 'Sport' | 'Yoga';

export interface Workout {
  id: number;
  exerciseType: ExerciseType;
  durationMinutes: number;
  caloriesBurned: number;
  intensity: number;
  fatigue: number;
  notes: string | null;
  performedAt: string;
}

export type WorkoutRequest = Omit<Workout, 'id'>;