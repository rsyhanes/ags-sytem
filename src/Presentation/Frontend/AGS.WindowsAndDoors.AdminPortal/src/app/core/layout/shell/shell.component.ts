import { Component } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './../header/header.component';
import { SidebarComponent } from './../sidebar/sidebar.component';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [MatSidenavModule, MatToolbarModule, RouterOutlet, HeaderComponent, SidebarComponent],
  template: `
    <mat-sidenav-container class="shell-container">
      <mat-sidenav mode="side" opened class="sidebar">
        <app-sidebar></app-sidebar>
      </mat-sidenav>

      <mat-sidenav-content class="main-content">
        <app-header></app-header>
        <main class="content-area">
          <router-outlet></router-outlet>
        </main>
      </mat-sidenav-content>
    </mat-sidenav-container>
  `,
  styles: [`
    .shell-container {
      height: 100vh;
    }

    .sidebar {
      width: 250px;
      background-color: #ffffff;
      border-right: 1px solid #e0e0e0;
      overflow: hidden !important;
    }

    .sidebar ::ng-deep .mat-drawer-inner-container {
      overflow: visible !important;
      display: flex;
      flex-direction: column;
    }

    .main-content {
      display: flex;
      flex-direction: column;
    }

    .content-area {
      flex: 1;
      padding: 24px;
      background-color: #f5f5f5;
      overflow-y: auto;
    }

    @media (max-width: 768px) {
      .sidebar {
        width: 200px;
      }
    }
  `]
})
export class ShellComponent {}
