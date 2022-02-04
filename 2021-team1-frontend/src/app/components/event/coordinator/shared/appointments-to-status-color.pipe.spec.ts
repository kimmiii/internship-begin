import { AppointmentsToStatusColorPipe } from './appointments-to-status-color.pipe';

describe('AppointmentsToStatusColorPipe', () => {
  it('create an instance', () => {
    const pipe = new AppointmentsToStatusColorPipe();
    expect(pipe).toBeTruthy();
  });
});
