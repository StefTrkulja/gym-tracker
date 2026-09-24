export interface UpdateProfileRequest {
  id: number;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  isGoogleAccount?: boolean;
}