import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from '../../../core/auth/auth.service';
import { UpdateProfileDialog } from '../../../features/profile/update-profile-dialog/update-profile-dialog';
import { UserService } from '../../../features/profile/user.service';

@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, RouterLinkActive, MatIconModule, MatButtonModule],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss',
})
export class Sidebar {
  private userService = inject(UserService)
  private authService = inject(AuthService);
  private router = inject(Router);
  private dialog = inject(MatDialog);

  currentUser = this.authService.currentUser;

 openSettings(): void {
    this.userService.getProfile().subscribe({
      next: (profile) => {
        const dialogRef = this.dialog.open(UpdateProfileDialog, {
          width: '480px',
          data: profile,
        });

        dialogRef.afterClosed().subscribe((result) => {
          if (result) {
            this.authService.fetchCurrentUser().subscribe();
          }
        });
      },
    });
  }
  logout(): void {
    this.authService.logout().subscribe({
      next: () => this.router.navigate(['/login']),
      error: () => this.router.navigate(['/login']),
    });
  }
}