import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/auth/auth.service';
import { MATERIAL_MODULES } from '../../../shared/material';

@Component({
  selector: 'app-register',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    ...MATERIAL_MODULES
  ],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  private authService = inject(AuthService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);


  isLoading = signal(false);
  hidePassword = signal(true);


  form = new FormGroup({
    firstName: new FormControl('', [Validators.required, Validators.maxLength(50)]),
    lastName: new FormControl('', [Validators.required, Validators.maxLength(50)]),
    username: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(50),
      Validators.pattern(/^[a-zA-Z0-9_.]{3,50}$/),
    ]),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(10),
      Validators.maxLength(72),
    ]),
  });

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    const value = this.form.getRawValue();

    this.authService.register({
      firstName: value.firstName!,
      lastName: value.lastName!,
      username: value.username!,
      email: value.email!,
      password: value.password!,
    }).subscribe({
      next: (success) => {
        this.isLoading.set(false);
        if (success) {
          this.snackBar.open('Welcome to GymTracker!', 'OK', { duration: 3000 });
          this.router.navigate(['/dashboard']);
        } else {
          this.snackBar.open('Registration succeeded, but failed to load profile. Try aagain.', 'OK', { duration: 5000 });
          this.router.navigate(['/login']);
        }
      },
      error: (err) => {
        this.isLoading.set(false);
        const message = err.error?.error ?? 'Registration failed. Please try again.';
        this.snackBar.open(message, 'OK', { duration: 5000 });
      },
    });
  }
}