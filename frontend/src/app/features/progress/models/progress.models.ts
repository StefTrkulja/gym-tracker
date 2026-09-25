export interface WeeklyStats {
  weekIndex: number;
  startDate: string;
  endDate: string;
  workoutCount: number;
  totalDurationMinutes: number;
	totalCaloriesBurned: number;
  averageIntensity: number | null;
  averageFatigue: number | null;
}

export interface MonthlyProgress {
  year: number;
  month: number;
  weeks: WeeklyStats[];
}