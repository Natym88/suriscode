import { HttpInterceptorFn } from '@angular/common/http';
import Swal from 'sweetalert2';

export const spinnerInterceptor: HttpInterceptorFn = (req, next) => {
  Swal.fire({
    title: "Cargando...",
    didOpen: () => {
      Swal.showLoading();
    }
  });
  return next(req);

}