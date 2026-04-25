import xml.etree.ElementTree as ET
import json
from datetime import datetime
import os
import sys


def cargar_datos():
    """Carga el JSON generado por el backend"""
    ruta = os.path.join(os.getcwd(), "data_export.json")

    if not os.path.exists(ruta):
        raise FileNotFoundError("No se encontró data_export.json")

    with open(ruta, "r", encoding="utf-8") as f:
        return json.load(f)


def generar_xml(clientes, reservas, facturas, estancias, nombre_lote="Lote", nombre_archivo=None):
    # Formato estándar de Odoo
    root = ET.Element("odoo")
    data_el = ET.SubElement(root, "data", noupdate="1")

    # clientes
    for c in clientes:
        record_id = f"hotel_cliente_{c.get('Id', '')}"
        record_el = ET.SubElement(data_el, "record", id=record_id, model="res.partner")

        nombre = str(c.get("Nombre", ""))
        apellido = str(c.get("Apellido", ""))
        nombre_completo = (nombre + " " + apellido).strip()
        ET.SubElement(record_el, "field", name="name").text = nombre_completo
        ET.SubElement(record_el, "field", name="vat").text = str(c.get("Documentacion", ""))
        ET.SubElement(record_el, "field", name="phone").text = str(c.get("Telefono", ""))
        # Puedes añadir un campo 'ref' para guardar el ID de tu sistema
        ET.SubElement(record_el, "field", name="ref").text = f"{nombre_lote}_ID{c.get('Id', '')}"

    # reservas
    for r in reservas:
        record_id = f"hotel_reserva_{r.get('Id', '')}"
        record_el = ET.SubElement(data_el, "record", id=record_id, model="hotel.reserva")
        
        # Usamos 'ref' para enlazar la reserva con el ID del cliente creado arriba
        ET.SubElement(record_el, "field", name="cliente_id", ref=f"hotel_cliente_{r.get('ClienteId', '')}")
        ET.SubElement(record_el, "field", name="fecha_entrada").text = str(r.get("FechaEntrada", ""))
        ET.SubElement(record_el, "field", name="fecha_salida").text = str(r.get("FechaSalida", ""))
        ET.SubElement(record_el, "field", name="habitacion").text = str(r.get("HabitacionId", ""))
        ET.SubElement(record_el, "field", name="estado").text = str(r.get("EstadoReservaId", ""))

    estancia_a_reserva = {e.get("Id"): e.get("ReservaId") for e in estancias}

    # facturas
    for f in facturas:
        record_id = f"hotel_factura_{f.get('Id', '')}"
        record_el = ET.SubElement(data_el, "record", id=record_id, model="hotel.factura")

        reserva_id = estancia_a_reserva.get(f.get("EstanciaId"))
        if reserva_id is not None:
            ET.SubElement(record_el, "field", name="reserva_id", ref=f"hotel_reserva_{reserva_id}")
        ET.SubElement(record_el, "field", name="total").text = str(f.get("Total", ""))
        ET.SubElement(record_el, "field", name="fecha").text = str(f.get("FechaEmision", ""))

    # guardar XML
    if not nombre_archivo:
        nombre_archivo = f"odoo_{datetime.now().strftime('%Y%m%d_%H%M%S')}.xml"
    tree = ET.ElementTree(root)
    
    # Indentación básica (Pretty Print) para que sea legible
    ET.indent(tree, space="    ", level=0)

    # Crear carpeta export si no existe
    carpeta_export = os.path.join(os.getcwd(), "export")
    os.makedirs(carpeta_export, exist_ok=True)

    ruta_salida = os.path.join(carpeta_export, nombre_archivo)
    tree.write(ruta_salida, encoding="utf-8", xml_declaration=True)

    print(f"Éxito: {nombre_archivo}")


if __name__ == "__main__":
    try:
        nombre_lote = sys.argv[1] if len(sys.argv) > 1 else "LoteManual"
        nombre_archivo = sys.argv[2] if len(sys.argv) > 2 else None
        
        data = cargar_datos()
        tables = data.get("Data", data)

        clientes = tables.get("Clientes", [])
        reservas = tables.get("Reservas", [])
        estancias = tables.get("Estancias", [])
        facturas = tables.get("Facturas", [])

        generar_xml(clientes, reservas, facturas, estancias, nombre_lote, nombre_archivo)

    except Exception as e:
        print(f"ERROR: {str(e)}")