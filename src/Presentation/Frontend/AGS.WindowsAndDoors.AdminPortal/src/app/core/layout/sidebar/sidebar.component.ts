import { Component, signal } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatExpansionModule } from '@angular/material/expansion';

interface NavItem {
  label: string;
  icon: string;
  route?: string;
  children?: NavItem[];
}

interface NavGroup {
  label: string;
  expanded: boolean;
  items: NavItem[];
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, MatListModule, MatIconModule, RouterModule, MatExpansionModule],
  template: `
    <nav class="sidebar-nav">
      <div class="nav-group" *ngFor="let group of navGroups()">
        <button class="group-header" (click)="toggleGroup(group)">
          <span class="group-label">{{ group.label }}</span>
          <mat-icon class="expand-icon" [class.expanded]="group.expanded">
            {{ group.expanded ? 'expand_more' : 'chevron_right' }}
          </mat-icon>
        </button>
        
        <div class="group-items" [class.expanded]="group.expanded">
          <a *ngFor="let item of group.items"
             class="nav-item"
             [routerLink]="item.route"
             routerLinkActive="active"
             [routerLinkActiveOptions]="{exact: false}">
            <mat-icon class="nav-icon">{{ item.icon }}</mat-icon>
            <span class="nav-label">{{ item.label }}</span>
          </a>
        </div>
      </div>
    </nav>
  `,
  styles: [`
    .sidebar-nav {
      height: 100%;
      padding: 16px 0;
      overflow-y: auto;
      overflow-x: hidden;
    }

    .nav-group {
      margin-bottom: 8px;
    }

    .group-header {
      width: 100%;
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 8px 16px;
      border: none;
      background: transparent;
      cursor: pointer;
      font-size: 12px;
      font-weight: 500;
      text-transform: uppercase;
      letter-spacing: 0.5px;
      color: #666;
      transition: background-color 0.2s ease;
    }

    .group-header:hover {
      background-color: rgba(0, 0, 0, 0.04);
    }

    .group-label {
      flex: 1;
      text-align: left;
    }

    .expand-icon {
      font-size: 20px;
      width: 20px;
      height: 20px;
      transition: transform 0.2s ease;
    }

    .expand-icon.expanded {
      transform: rotate(0deg);
    }

    .group-items {
      max-height: 0;
      overflow: hidden;
      transition: max-height 0.3s ease;
    }

    .group-items.expanded {
      max-height: 500px;
    }

    .nav-item {
      display: flex;
      align-items: center;
      padding: 12px 16px 12px 32px;
      text-decoration: none;
      color: #333;
      transition: all 0.2s ease;
      border-left: 3px solid transparent;
      cursor: pointer;
    }

    .nav-item:hover {
      background-color: rgba(0, 0, 0, 0.04);
    }

    .nav-item.active {
      background-color: #f5f5f5;
      border-left-color: #1976d2;
      color: #1976d2;
    }

    .nav-item.active .nav-icon {
      color: #1976d2;
    }

    .nav-icon {
      font-size: 20px;
      width: 20px;
      height: 20px;
      margin-right: 12px;
      color: #666;
    }

    .nav-label {
      font-size: 14px;
      font-weight: 400;
      line-height: 20px;
    }

    /* Scrollbar styling - hide by default, show on hover when needed */
    .sidebar-nav {
      scrollbar-width: none; /* Firefox - hide scrollbar */
    }

    .sidebar-nav:hover {
      scrollbar-width: thin; /* Firefox - show on hover */
      scrollbar-color: #ccc transparent;
    }

    /* Webkit browsers - hide scrollbar by default */
    .sidebar-nav::-webkit-scrollbar {
      width: 0;
      height: 0;
    }

    /* Webkit browsers - show scrollbar on hover */
    .sidebar-nav:hover::-webkit-scrollbar {
      width: 6px;
    }

    .sidebar-nav::-webkit-scrollbar-track {
      background: transparent;
    }

    .sidebar-nav::-webkit-scrollbar-thumb {
      background: #ccc;
      border-radius: 3px;
    }

    .sidebar-nav::-webkit-scrollbar-thumb:hover {
      background: #999;
    }
  `]
})
export class SidebarComponent {
  navGroups = signal<NavGroup[]>([
    {
      label: 'Management',
      expanded: true,
      items: [
        { label: 'Dashboard', icon: 'dashboard', route: '/dashboard' },
        { label: 'Orders', icon: 'shopping_cart', route: '/orders' },
        { label: 'Users', icon: 'people', route: '/users' }
      ]
    },
    {
      label: 'Catalog',
      expanded: true,
      items: [
        { label: 'Systems', icon: 'build', route: '/systems' },
        { label: 'Items', icon: 'inventory', route: '/items' }
      ]
    }
  ]);

  toggleGroup(group: NavGroup): void {
    group.expanded = !group.expanded;
    this.navGroups.set([...this.navGroups()]);
  }
}
