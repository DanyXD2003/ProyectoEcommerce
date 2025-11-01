export interface Direccion {
  id: number;
  usuarioId: number;
  calle: string;
  ciudad: string;
  departamento?: string;
  codigoPostal?: string;
  pais?: string;
  telefono?: string;
}
