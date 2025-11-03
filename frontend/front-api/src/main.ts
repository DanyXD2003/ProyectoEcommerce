// main.ts
import { bootstrapApplication } from '@angular/platform-browser';
import { App } from './app/app';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';

// ✅ Importación correcta
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './app/core/interceptors/auth.interceptor';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

bootstrapApplication(App, {
  providers: [
    provideRouter(routes),

    // ✅ Usar interceptores inyectados desde DI
    provideHttpClient(
      withInterceptorsFromDi()
    ),

    provideAnimationsAsync(),

    // ✅ Registrar tu interceptor basado en clase
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ]
});
