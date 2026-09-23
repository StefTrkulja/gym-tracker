import { Component, inject } from '@angular/core';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MATERIAL_MODULES } from '../../../shared/material';
import { UpdateProfileRequest } from '../models/user.models';
import { UserService } from '../user.service';
@Component({
  selector: 'app-update-profile-dialog',
  imports: [ReactiveFormsModule, ...MATERIAL_MODULES],
  templateUrl: './update-profile-dialog.html',
  styleUrl: './update-profile-dialog.scss',
})
export class UpdateProfileDialog {
 
  private userService = inject(UserService);
  private snackBar = inject(MatSnackBar);
  private dialogRef = inject(MatDialogRef<UpdateProfileDialog>);
  
  data = inject<UpdateProfileRequest>(MAT_DIALOG_DATA);

  isLoading = false

   form = new FormGroup({
    username: new FormControl(this.data.username, [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(50),
      Validators.pattern(/^[a-zA-Z0-9_.]{3,50}$/),
    ]),
    email: new FormControl(this.data.email, [Validators.required, Validators.email]),
    firstName: new FormControl(this.data.firstName, [Validators.required, Validators.maxLength(50)]),
    lastName: new FormControl(this.data.lastName, [Validators.required, Validators.maxLength(50)]),
  });

    onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const value = this.form.getRawValue();
  
    this.userService.updateProfile({
      id: this.data.id,
      username: value.username!,
      email: value.email!,
      firstName: value.firstName!,
      lastName: value.lastName!,
    }).subscribe({
      next: () => {
        this.isLoading = false;
        this.snackBar.open('Profile updated.', 'OK', { duration: 3000 });
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.isLoading = false;
        const message = err.error?.error ?? 'Failed to update profile.';
        this.snackBar.open(message, 'OK', { duration: 5000 });
      },
    });
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
