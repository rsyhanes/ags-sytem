import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: '/items'
  },
  {
    path: 'items',
    loadChildren: () => import('./features/items').then(m => m.ITEMS_ROUTES)
  }
];
