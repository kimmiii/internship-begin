import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'appointmentsToStatusColor'
})
export class AppointmentsToStatusColorPipe implements PipeTransform {

  transform(amountOfAppointments: number): string {
    if (amountOfAppointments >= 5) {
      return 'report-grid__student--success';
    } else if (amountOfAppointments > 0) {
      return 'report-grid__student--warning';
    } else {
      return 'report-grid__student--error';
    }
  }

}
