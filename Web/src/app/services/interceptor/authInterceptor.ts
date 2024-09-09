import { HttpErrorResponse, HttpEvent, HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { map, catchError, throwError, finalize } from 'rxjs';
import Swal from 'sweetalert2';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  
    const reqCloned = req.clone({
        setHeaders: {
          'Access-Control-Allow-Origin': '*'
        }
    });
      
      
    return next(reqCloned).pipe(
        map((event: HttpEvent<any>) => {
            if (event instanceof HttpResponse)
            Swal.close();
            return event;
            }),
        catchError((err: any) => {
            Swal.close();
          if (err instanceof HttpErrorResponse) {
            // Handle HTTP errors
            if (err.status === 401) {
              console.error('Unauthorized request:', err);
            } else {
              // Mostrar el mensaje de error usando Swal
                Swal.fire({
                icon: 'error',
                title: 'Error',
                text: err.error.detail,
                showConfirmButton: true
                });
            }
          } else {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: "Error desconocido",
                showConfirmButton: true
                });
            console.error('An error occurred:', err);
          }
    
          return throwError(() => err); 
        }),
        finalize(() => {
            
        })
    );
}
