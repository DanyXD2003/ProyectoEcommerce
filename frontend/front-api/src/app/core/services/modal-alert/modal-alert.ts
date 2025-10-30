// alert-modal.component.ts
import { Component, Inject } from '@angular/core';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

@Component({
  standalone: true,
  selector: 'app-alert-modal',
  imports: [CommonModule, MatDialogModule, MatButtonModule],
  template: `
    <div class="text-center">
      <h2 class="alert-modal-title text-align-center">{{ data.title }}</h2>
      <p class="alert-modal-message">{{ data.message }}</p>
      <div class="actions">
          <button mat-button class="alert-modal-ok-button" (click)="close()">OK</button>
      </div>
    </div>
    
  `,
  styleUrls: ['./modal-alert.css'],
})

export class AlertModalComponent {
  constructor(
    private dialogRef: MatDialogRef<AlertModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { title: string; message: string }
  ) {}

  close() {
    this.dialogRef.close();
  }
}
