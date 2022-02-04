import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RegisteredCompanyService {
  isCompanyRegistered: Subject<boolean> = new Subject<boolean>();
}
