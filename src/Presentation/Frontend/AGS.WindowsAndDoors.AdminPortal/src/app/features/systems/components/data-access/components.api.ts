import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';

export interface ComponentDto {
  id: string;
  systemCode: string;
  itemCode: string;
  name: string;
  description: string;
  quantity: number;
  dimensions?: {
    lengthFormula?: string;
    fixedLength?: { value: number; unit: string };
  };
  isRequired: boolean;
  sortOrder: number;
  createdAt: string;
}

export interface CreateComponentRequest {
  itemCode: string;
  name: string;
  quantity: number;
  description?: string;
  lengthFormula?: string;
  fixedLengthValue?: number;
  fixedLengthUnit?: string;
  isRequired?: boolean;
  sortOrder?: number;
}

export interface UpdateComponentRequest {
  name?: string;
  quantity?: number;
  description?: string;
  lengthFormula?: string;
  fixedLengthValue?: number;
  fixedLengthUnit?: string;
  isRequired?: boolean;
  sortOrder?: number;
}

@Injectable({
  providedIn: 'root'
})
export class ComponentsApiService {
  private http = inject(HttpClient);

  getComponents(systemCode: string): Observable<ComponentDto[]> {
    return this.http.get<ComponentDto[]>(`/api/systems/${systemCode}/components`)
      .pipe(catchError(this.handleError));
  }

  getComponent(id: string): Observable<ComponentDto> {
    return this.http.get<ComponentDto>(`/api/components/${id}`)
      .pipe(catchError(this.handleError));
  }

  createComponent(systemCode: string, component: CreateComponentRequest): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(`/api/systems/${systemCode}/components`, component)
      .pipe(catchError(this.handleError));
  }

  updateComponent(id: string, updates: UpdateComponentRequest): Observable<ComponentDto> {
    return this.http.put<ComponentDto>(`/api/components/${id}`, updates)
      .pipe(catchError(this.handleError));
  }

  deleteComponent(id: string): Observable<void> {
    return this.http.delete<void>(`/api/components/${id}`)
      .pipe(catchError(this.handleError));
  }

  private handleError = (error: HttpErrorResponse): Observable<never> => {
    console.error('Components API Error:', error);

    if (error.status === 400) {
      return throwError(() => new Error(error.error?.message || 'Validation error'));
    }
    if (error.status === 404) {
      return throwError(() => new Error('Component not found'));
    }
    if (error.status === 409) {
      return throwError(() => new Error(error.error?.message || 'Business rule violation'));
    }
    if (error.status === 500) {
      return throwError(() => new Error('Server error occurred'));
    }

    return throwError(() => new Error('An unexpected error occurred'));
  };
}
