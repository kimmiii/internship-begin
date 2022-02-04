import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(
    private snackBar: MatSnackBar,
  ) { }

  showErrorSnackBar(text: string): void {
    this.snackBar.open(text, 'OK', {
      panelClass: [ 'snackbar', 'error-snackbar' ],
    });
  }

  showSuccessSnackBar(text: string): void {
    this.snackBar.open(text, null, {
      duration: 3000,
      panelClass: [ 'snackbar', 'success-snackbar' ],
    });
  }
}
