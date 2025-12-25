import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatMenuModule
  ],
  template: `
    <mat-toolbar class="header-toolbar">
      <div class="logo-section">
        <span class="logo-text">AGS</span>
      </div>

      <div class="search-section">
        <mat-form-field appearance="outline" class="search-field">
          <mat-label>Search</mat-label>
          <input matInput type="text" placeholder="Search...">
          <mat-icon matSuffix>search</mat-icon>
        </mat-form-field>
      </div>

      <div class="actions-section">
        <button mat-icon-button class="notification-btn">
          <mat-icon>notifications</mat-icon>
          <span class="notification-badge">3</span>
        </button>

        <button mat-icon-button [matMenuTriggerFor]="userMenu">
          <mat-icon>account_circle</mat-icon>
        </button>
        <mat-menu #userMenu="matMenu">
          <button mat-menu-item>
            <mat-icon>person</mat-icon>
            <span>Profile</span>
          </button>
          <button mat-menu-item>
            <mat-icon>settings</mat-icon>
            <span>Settings</span>
          </button>
          <button mat-menu-item>
            <mat-icon>logout</mat-icon>
            <span>Logout</span>
          </button>
        </mat-menu>
      </div>
    </mat-toolbar>
  `,
  styles: [`
    .header-toolbar {
      height: 60px;
      background-color: #f5f5f5;
      border-bottom: 1px solid #e0e0e0;
      padding: 0 24px;
      display: flex;
      align-items: center;
      justify-content: space-between;
    }

    .logo-section {
      display: flex;
      align-items: center;
    }

    .logo-text {
      font-size: 24px;
      font-weight: bold;
      color: #000;
    }

    .search-section {
      flex: 1;
      max-width: 400px;
      margin: 0 40px;
    }

    .search-field {
      width: 100%;
    }

    .actions-section {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .notification-btn {
      position: relative;
    }

    .notification-badge {
      position: absolute;
      top: 8px;
      right: 8px;
      background-color: #f44336;
      color: white;
      border-radius: 50%;
      width: 16px;
      height: 16px;
      font-size: 10px;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    @media (max-width: 768px) {
      .header-toolbar {
        padding: 0 16px;
      }

      .search-section {
        margin: 0 16px;
        max-width: 200px;
      }
    }
  `]
})
export class HeaderComponent {}
