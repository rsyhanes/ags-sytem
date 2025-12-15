import { Routes } from '@angular/router';
import { ItemsListPage } from './pages/items-list.page';
import { ItemFormPage } from './pages/item-form.page';

export const ITEMS_ROUTES: Routes = [
  {
    path: '',
    //loadComponent: () => import('./pages/items-list.page').then(m => m.ItemsListPage),
    component: ItemsListPage,
    title: 'Items'
  },
  {
    path: 'new',
    //loadComponent: () => import('./pages/item-form.page').then(m => m.ItemFormPage),
    component: ItemFormPage,
    title: 'New Item'
  },
  {
    path: ':code/edit',
    //loadComponent: () => import('./pages/item-form.page').then(m => m.ItemFormPage),
    component: ItemFormPage,
    title: 'Edit Item'
  }
];
