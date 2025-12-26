import { patchState, signalStore, withComputed, withMethods, withState } from '@ngrx/signals';
import { computed, inject } from '@angular/core';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { ComponentsApiService, ComponentDto, CreateComponentRequest, UpdateComponentRequest } from './components.api';
import { catchError, of, tap, switchMap, pipe } from 'rxjs';

// State interface
export interface ComponentsState {
  components: ComponentDto[];
  loading: boolean;
  error: string | null;
  selectedSystemCode: string | null;
}

// Initial state
const initialState: ComponentsState = {
  components: [],
  loading: false,
  error: null,
  selectedSystemCode: null
};

// SignalStore
export const ComponentsStore = signalStore(
  { providedIn: 'root' },
  withState<ComponentsState>(initialState),
  withComputed(store => ({
    // Computed properties for derived state
    hasComponents: computed(() => store.components().length > 0),
    componentCount: computed(() => store.components().length),
    isEmpty: computed(() => store.components().length === 0),
    requiredComponents: computed(() =>
      store.components().filter(c => c.isRequired)
    ),
    optionalComponents: computed(() =>
      store.components().filter(c => !c.isRequired)
    )
  })),
  withMethods(store => {
    const api = inject(ComponentsApiService);

    return {
      // Synchronous mutations (direct state changes)
      setLoading: (loading: boolean) => patchState(store, { loading }),
      setError: (error: string | null) => patchState(store, { error }),
      setSystemCode: (systemCode: string) => patchState(store, { selectedSystemCode: systemCode }),
      addComponent: (component: ComponentDto) => patchState(store, state => ({
        components: [...state.components, component]
      })),
      updateComponent: (component: ComponentDto) => patchState(store, state => ({
        components: state.components.map(existing =>
          existing.id === component.id ? component : existing
        )
      })),
      removeComponent: (id: string) => patchState(store, state => ({
        components: state.components.filter(component => component.id !== id)
      })),
      clearError: () => patchState(store, { error: null }),

      // Async operations using rxMethod following Ngrx Signal Store pattern
      loadComponents: rxMethod<string>(pipe(
        tap(systemCode => patchState(store, { selectedSystemCode: systemCode, loading: true, error: null })),
        switchMap(systemCode => api.getComponents(systemCode).pipe(
          tap(components => patchState(store, { components, loading: false })),
          catchError(error => {
            patchState(store, { components: [], loading: false, error: error.message });
            return of([]);
          })
        ))
      )),

      createComponent: rxMethod<CreateComponentRequest>(request$ =>
        request$.pipe(
          tap(() => patchState(store, { loading: true, error: null })),
          switchMap(request => {
            const systemCode = store.selectedSystemCode();
            if (!systemCode) {
              patchState(store, { loading: false, error: 'No system selected' });
              return of(null);
            }
            return api.createComponent(systemCode, request).pipe(
              // After successful creation, reload components to get the full data
              switchMap(result => api.getComponents(systemCode).pipe(
                tap(components => patchState(store, { components, loading: false })),
                catchError(error => {
                  patchState(store, { loading: false, error: error.message });
                  return of([]);
                })
              )),
              catchError(error => {
                patchState(store, { loading: false, error: error.message });
                return of(null);
              })
            );
          })
        )
      ),

      updateComponentById: rxMethod<[string, UpdateComponentRequest]>(params$ =>
        params$.pipe(
          tap(() => patchState(store, { loading: true, error: null })),
          switchMap(([id, updates]) => api.updateComponent(id, updates).pipe(
            tap(updatedComponent => patchState(store, state => ({
              components: state.components.map(component =>
                component.id === updatedComponent.id ? updatedComponent : component
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

      deleteComponentById: rxMethod<string>(id$ =>
        id$.pipe(
          tap(() => patchState(store, { loading: true, error: null })),
          switchMap(id => api.deleteComponent(id).pipe(
            tap(() => patchState(store, state => ({
              components: state.components.filter(component => component.id !== id),
              loading: false
            }))),
            catchError(error => {
              patchState(store, { loading: false, error: error.message });
              return of(null);
            })
          ))
        )
      ),

      getComponent: rxMethod<string>(id$ =>
        id$.pipe(
          tap(() => patchState(store, { loading: true, error: null })),
          switchMap(id => api.getComponent(id).pipe(
            tap(() => patchState(store, { loading: false })),
            catchError(error => {
              patchState(store, { loading: false, error: error.message });
              return of(null);
            })
          )))
      )
    };
  })
);
