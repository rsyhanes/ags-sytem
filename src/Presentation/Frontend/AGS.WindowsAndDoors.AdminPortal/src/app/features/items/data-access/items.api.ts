import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';

export interface ItemDto {
  code: string;
  name: string;
  description: string;
  price: number;
  dimensions?: { value: number; unit: string };
  color?: { name: string; hex: string };
  state: string;
}

export interface CreateItemRequest {
  code: string;
  name: string;
  description: string;
  price: number;
  measure: 'Units' | 'Inches';
}

export interface UpdateItemRequest {
  name?: string;
  measure?: 'Units' | 'Inches';
}

@Injectable({
  providedIn: 'root'
})
export class ItemsApiService {
  private http = inject(HttpClient);
  private baseUrl = '/api/items';

  getItems(): Observable<ItemDto[]> {
    return this.http.get<ItemDto[]>(this.baseUrl)
      .pipe(catchError(this.handleError));
  }

  getItem(code: string): Observable<ItemDto> {
    return this.http.get<ItemDto>(`${this.baseUrl}/${code}`)
      .pipe(catchError(this.handleError));
  }

  createItem(item: CreateItemRequest): Observable<ItemDto> {
    const apiItem = {
      code: item.code.trim().toUpperCase(),
      name: item.name.trim(),
      description: item.description,
      price: item.price,
      dimensionValue: null,
      dimensionUnit: item.measure,
      colorName: null,
      colorHex: null
    };

    return this.http.post<ItemDto>(this.baseUrl, apiItem)
      .pipe(catchError(this.handleError));
  }

  updateItem(code: string, updates: UpdateItemRequest): Observable<ItemDto> {
    const apiUpdate: any = {};
    if (updates.name !== undefined) {
      apiUpdate.name = updates.name.trim();
    }
    if (updates.measure !== undefined) {
      apiUpdate.dimensionUnit = updates.measure;
    }

    return this.http.put<ItemDto>(`${this.baseUrl}/${code}`, apiUpdate)
      .pipe(catchError(this.handleError));
  }

  deleteItem(code: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${code}`)
      .pipe(catchError(this.handleError));
  }

  private handleError = (error: HttpErrorResponse): Observable<never> => {
    console.error('API Error:', error);
    if (error.status === 409) {
      return throwError(() => new Error(error.error?.message || 'Item code already exists'));
    }
    if (error.status === 404) {
      return throwError(() => new Error('Item not found'));
    }
    return throwError(() => new Error('An unexpected error occurred'));
  };
}
