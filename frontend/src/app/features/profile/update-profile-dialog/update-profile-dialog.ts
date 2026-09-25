import { Component, inject } from '@angular/core';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MATERIAL_MODULES } from '../../../shared/material';
import {UserProfile } from '../models/user.models';
import { UserService } from '../user.service';
import { signal } from '@angular/core';

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

  data = inject<UserProfile>(MAT_DIALOG_DATA);

  isLoading = signal<boolean>(false);
  isGoogleAccount = this.data.isGoogleAccount ?? false;

  form = new FormGroup({
    username: new FormControl(this.data.username, [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(50),
      Validators.pattern(/^[a-zA-Z0-9_.]{3,50}$/),
    ]),
    email: new FormControl({ value: this.data.email, disabled: this.isGoogleAccount }, [Validators.required, Validators.email]),
    firstName: new FormControl(this.data.firstName, [Validators.required, Validators.maxLength(50)]),
    lastName: new FormControl(this.data.lastName, [Validators.required, Validators.maxLength(50)]),
  });

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    const value = this.form.getRawValue();

    this.userService.updateProfile({
      username: value.username!,
      email: value.email!,
      firstName: value.firstName!,
      lastName: value.lastName!,
    }).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.snackBar.open('Profile updated.', 'OK', { duration: 3000 });
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.isLoading.set(false);
        const message = err.error?.error ?? 'Failed to update profile.';
        this.snackBar.open(message, 'OK', { duration: 5000 });
      },
    });
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
