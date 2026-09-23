import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/auth/auth.service';
import { MATERIAL_MODULES } from '../../../shared/material';


@Component({
  selector: 'app-login',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
   ...MATERIAL_MODULES
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  private authService = inject(AuthService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  isLoading = false;
  hidePassword = true;

  form = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
  });

onSubmit(): void {
  if (this.form.invalid) {
    this.form.markAllAsTouched();
    return;
  }

  this.isLoading = true;
  const value = this.form.getRawValue();

  this.authService.login({
    email: value.email!,
    password: value.password!,
  }).subscribe({
    next: (success) => {
      this.isLoading = false;
      if (success) {
        this.router.navigate(['/dashboard']);
      } else {
        this.snackBar.open('Login failed. Please try again.', 'OK', { duration: 5000 });
      }
    },
    error: (err) => {
      this.isLoading = false;
      const message = err.error?.error ?? 'Invalid email or password.';
      this.snackBar.open(message, 'OK', { duration: 5000 });
    },
  });
}
}