export interface UpdateProfileRequest {
  username: string;
  email: string;
  firstName: string;
  lastName: string;
}


export interface UserProfile {
  id: number;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  isGoogleAccount: boolean;

}