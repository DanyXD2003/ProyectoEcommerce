import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

interface Producto {
  nombre: string;
  precio: string;
  imagen: string;
}

@Component({
  selector: 'app-productos',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './productos.html',
  styleUrls: ['./productos.css'],
})

export class productos {
  categorias = ['Electrónica', 'Ropa', 'Hogar', 'Libros', 'Deportes'];

  productos: Producto[] = [
    { nombre: 'Auriculares Inalámbricos', precio: '$129.99', imagen: 'https://lh3.googleusercontent.com/aida-public/AB6AXuCpqp3AQ1WqMjvZnI_6Pc1nQjFBwtJu4KVKt0s8V2qjWwX__Qm15fjKUftZZpfiQJSqScxAayjfAlBBCPzfvlXkRCIiqvZ9AQPL9aApYN38YVSzWOb-UzgFwq6ScGWTXGbSjoG_UgcT0enZCGktVM9DOKrjVebUGt0b8sZwwRl0wuSEPuailu8gOdxFoajULysXWHbdSQsWGVmlZ94l0j5-PI6y3grbujrgYgi9PJTP4AZ1xf57MDzVTq1LS1-2w-YXXZG2Wc8MPqY' },
    { nombre: 'Smartwatch Pro', precio: '$249.99', imagen: 'https://lh3.googleusercontent.com/aida-public/AB6AXuANcpnTZdEPyz2lBTvlP71WUzcxpBQxuqk4WrCwboj-Me8DuJ7awCgiLgGGPKtNaiAP7pkCp9X89UwSqYC8cGJ-hgh6S83YqToXYRhT3G-u7epfrGCbC4M2daL0_WRovo1OcqNBAqz4od9QCE9PhmJVvE4ircKDRKXx5V7BADyQCHr95CEBl35Fj1qHSQO8j8u-semao_M_bYo859d0qYrIELbQxUxWo4uqiS3KvE40SQtD2vgIK8iXVeXx92GnZILCK1RPBpAQhfk' },
    { nombre: 'Tablet Gráfica', precio: '$89.50', imagen: 'https://lh3.googleusercontent.com/aida-public/AB6AXuAuBkTgjyZZq5H8MkEGrhES8ty3E8rD1E6uqkUlQvgfznoFvVsvVH9bIC6Bf2wkGjrb7z16F3GVlCzI2ud7GapKicTnkg1pDB57wGtVoBitqfqWdHE8avmmH_5pYiDn10-9RZ6GyQoDMnohJPg9dl5yL8eTv_e1RIqFabpOjFDa8MjY1WLuRQUUtUnUcOtqlvKu93sp7ZNiZfsL60gUqULRVfNUEKKhmlFJiYQHEnKVL2oXeDbMoog5HBRCp0hWxwb-QVE28P4_C6k' },
    { nombre: 'Teclado Mecánico', precio: '$99.00', imagen: 'https://lh3.googleusercontent.com/aida-public/AB6AXuAiFOWuvsTZqg8hpeSOW3k4pgtVt2yDBK1v_nk5vjIoD-mdL7L6Kf-CcCOM8uNLOzgKFPiLtLoKD1qf8T674-RVqG4WwLmKqQe8yLtuRxhrB86ZLwMJooQKYWuKYhuAProaO-9oJ0eO63Yjf41_vCEHXB885Rc1P-UlzsnZ4W88W5FXUqRuCiQlctP6dzum8Q0V6z5c7HnMX7laFhNaZk3MlmruitryGilhJLpeYFGWABO5sVge4h693ZYgS3GSCSJwayM-PNkwh9s' }
  ];
}
