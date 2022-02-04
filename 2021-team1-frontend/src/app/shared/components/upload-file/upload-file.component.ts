import { Component, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html',
  styleUrls: [ './upload-file.component.scss' ],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => UploadFileComponent),
      multi: true,
    },
  ],
})
export class UploadFileComponent implements ControlValueAccessor {

  @Input() label: string;
  @Input() isImage?: boolean;

  disabled: boolean;
  fileName: string;

  readonly browseFilePlaceholder = 'Kies bestand...';
  readonly maxSizeInBytes = 2000000;
  readonly pdfType = 'application/pdf';
  readonly jpgType = 'image/jpeg';

  onChanged: (file: File) => void;
  onTouched: () => void;

  constructor(
    private notificationService: NotificationService,
  ) {
  }

  registerOnChange(fn: (file: File) => void): void {
    this.onChanged = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  writeValue(): void {
    // never used
  }

  upload(files: FileList): void {
    if (this.isImage) {
      this.uploadLogo(files);
    } else {
      this.uploadPdfFile(files);
    }
  }

  uploadPdfFile(files: FileList): void {
    const file = files.item(0);
    if (file && file.type !== this.pdfType) {
      this.notificationService.showErrorSnackBar('Het bestand moet een PDF-bestand zijn');
    } else if (file && file.size >= this.maxSizeInBytes) {
      this.notificationService.showErrorSnackBar('Het bestand mag slechts 2MB groot zijn');
    } else {
      this.fileName = file ? file.name : this.browseFilePlaceholder;
      this.onChanged(file);
    }
  }

  uploadLogo(files: FileList): void {
    const file = files.item(0);
    if (file && file.type !== this.jpgType) {
      this.notificationService.showErrorSnackBar('Het bestand moet een JPG-bestand zijn');
    } else if (file && file.size >= this.maxSizeInBytes) {
      this.notificationService.showErrorSnackBar('Het bestand mag slechts 2MB groot zijn');
    } else {
      this.fileName = file ? file.name : this.browseFilePlaceholder;
      this.onChanged(file);
    }
  }
}
