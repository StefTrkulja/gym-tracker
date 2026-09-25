import { AfterViewInit, Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/auth/auth.service';
import { MATERIAL_MODULES } from '../../../shared/material';
declare const google: any;
import { environment } from '../../../../env/environment';

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
export class Login implements AfterViewInit {
  private authService = inject(AuthService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  isLoading = signal(false);
  hidePassword = signal(true);

  form = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
  });

  ngAfterViewInit(): void {
    google.accounts.id.initialize({
      client_id: environment.googleClientId,
      callback: (response: any) => this.handleGoogleLogin(response),
    });
    google.accounts.id.renderButton(
      document.getElementById('google-login-btn'),
      { theme: 'outline', size: 'large', width: 320 }
    );
  }

  handleGoogleLogin(response: any): void {
    this.isLoading.set(true);
    this.authService.loginWithGoogle(response.credential).subscribe({
      next: (success) => {
        this.isLoading.set(false);
        if (success) {
          this.router.navigate(['/dashboard']);
        } else {
          this.snackBar.open('Google login failed.', 'OK', { duration: 5000 });
        }
      },
      error: () => {
        this.isLoading.set(false);
        this.snackBar.open('Google login failed.', 'OK', { duration: 5000 });
      },
    });
  }


  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

        this.isLoading.set(true);
    const value = this.form.getRawValue();

    this.authService.login({
      email: value.email!,
      password: value.password!,
    }).subscribe({
      next: (success) => {
        this.isLoading.set(false);
        if (success) {
          this.router.navigate(['/dashboard']);
        } else {
          this.snackBar.open('Login failed. Please try again.', 'OK', { duration: 5000 });
        }
      },
      error: (err) => {
        this.isLoading.set(false);
        const message = err.error?.error ?? 'Invalid email or password.';
        this.snackBar.open(message, 'OK', { duration: 5000 });
      },
    });
  }
}