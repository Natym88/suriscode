/*
*  Protractor support is deprecated in Angular.
*  Protractor is used in this example for compatibility with Angular documentation tools.
*/
import { bootstrapApplication,provideProtractorTestingSupport } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './app/services/interceptor/authInterceptor';
import { spinnerInterceptor } from './app/services/interceptor/spinner';

bootstrapApplication(AppComponent,
    {providers: [
      provideProtractorTestingSupport(), 
      provideAnimationsAsync(), 
      provideHttpClient( withInterceptors([spinnerInterceptor, authInterceptor])),
    ]})
  .catch(err => console.error(err));
