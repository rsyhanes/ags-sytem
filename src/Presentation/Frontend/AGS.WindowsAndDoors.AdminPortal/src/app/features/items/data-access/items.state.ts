import { patchState, signalStore, withComputed, withMethods, withState } from '@ngrx/signals';
import { computed, inject } from '@angular/core';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { ItemsApiService, ItemDto, CreateItemRequest, UpdateItemRequest } from './items.api';
import { catchError, of, tap, switchMap, pipe } from 'rxjs';

// State interface
export interface ItemsState {
  items: ItemDto[];
  loading: boolean;
  error: string | null;
}

// Initial state
const initialState: ItemsState = {
  items: [],
  loading: false,
  error: null
};

// SignalStore
export const ItemsStore = signalStore(
  { providedIn: 'root' },
  withState<ItemsState>(initialState),
  withComputed(store => ({
    // Computed properties for derived state
    hasItems: computed(() => store.items().length > 0),
    itemCount: computed(() => store.items().length),
    isEmpty: computed(() => store.items().length === 0)
  })),
  withMethods(store => {
    const api = inject(ItemsApiService);

    return {
      // Synchronous mutations (direct state changes)
      setLoading: (loading: boolean) => patchState(store, { loading }),
      setError: (error: string | null) => patchState(store, { error }),
      addItem: (item: ItemDto) => patchState(store, state => ({
        items: [...state.items, item]
      })),
      updateItem: (item: ItemDto) => patchState(store, state => ({
        items: state.items.map(existing =>
          existing.code === item.code ? item : existing
        )
      })),
      removeItem: (code: string) => patchState(store, state => ({
        items: state.items.filter(item => item.code !== code)
      })),
      clearError: () => patchState(store, { error: null }),

      // Async operations using rxMethod following Ngrx Signal Store pattern
      loadItems: rxMethod<void>(pipe(
        tap(() => patchState(store, { loading: true, error: null })),        
        switchMap(() => api.getItems().pipe(
          tap(items => patchState(store, { items, loading: false })),
          catchError(error => {
            patchState(store, { items: [], loading: false, error: error.message });
            return of([]);
          })
        ))
      )),

      createItem: rxMethod<CreateItemRequest>(request$ =>
        request$.pipe(
          tap(() => patchState(store, { loading: true, error: null })),
          switchMap(request => api.createItem(request).pipe(
            tap(newItem => patchState(store, state => ({
              items: [...state.items, newItem],
              loading: false
            }))),
            catchError(error => {
              patchState(store, { loading: false, error: error.message });
              return of(null);
            })
          ))
        )
      ),

      updateItemByCode: rxMethod<[string, UpdateItemRequest]>(params$ =>
        params$.pipe(
          tap(() => patchState(store, { loading: true, error: null })),
          switchMap(([code, updates]) => api.updateItem(code, updates).pipe(
            tap(updatedItem => patchState(store, state => ({
              items: state.items.map(item =>
                item.code === updatedItem.code ? updatedItem : item
              ),
              loading: false
            }))),
            catchError(error => {
              patchState(store, { loading: false, error: error.message });
              return of(null);
            })
          ))
        )
      ),

      deleteItemByCode: rxMethod<string>(code$ =>
        code$.pipe(
          tap(() => patchState(store, { loading: true, error: null })),
          switchMap(code => api.deleteItem(code).pipe(
            tap(() => patchState(store, state => ({
              items: state.items.filter(item => item.code !== code),
              loading: false
            }))),
            catchError(error => {
              patchState(store, { loading: false, error: error.message });
              return of(null);
            })
          ))
        )
      ),

      getItem: rxMethod<string>(code$ =>
        code$.pipe(
          tap(() => patchState(store, { loading: true, error: null })),
          switchMap(code => api.getItem(code).pipe(
            tap(() => patchState(store, { loading: false })),
            catchError(error => {
              patchState(store, { loading: false, error: error.message });
              return of(null);
            })
          ))
        )
      )
    };
  })
);
