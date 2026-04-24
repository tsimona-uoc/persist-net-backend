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


def generar_xml(clientes, reservas, facturas, nombre_lote="Lote"):
    # Formato estándar de Odoo
    root = ET.Element("odoo")
    data_el = ET.SubElement(root, "data", noupdate="1")

    # clientes 
    for c in clientes:
        record_id = f"hotel_cliente_{c.get('id', '')}"
        record_el = ET.SubElement(data_el, "record", id=record_id, model="res.partner")

        ET.SubElement(record_el, "field", name="name").text = str(c.get("nombre", ""))
        ET.SubElement(record_el, "field", name="vat").text = str(c.get("documento", ""))
        ET.SubElement(record_el, "field", name="phone").text = str(c.get("telefono", ""))
        # Puedes añadir un campo 'ref' para guardar el ID de tu sistema
        ET.SubElement(record_el, "field", name="ref").text = f"{nombre_lote}_ID{c.get('id', '')}"

    # reservas 
    for r in reservas:
        record_id = f"hotel_reserva_{r.get('id', '')}"
        record_el = ET.SubElement(data_el, "record", id=record_id, model="hotel.reserva")
        
        # Usamos 'ref' para enlazar la reserva con el ID del cliente creado arriba
        ET.SubElement(record_el, "field", name="cliente_id", ref=f"hotel_cliente_{r.get('clienteId', '')}")
        ET.SubElement(record_el, "field", name="fecha_entrada").text = str(r.get("fechaEntrada", ""))
        ET.SubElement(record_el, "field", name="fecha_salida").text = str(r.get("fechaSalida", ""))
        ET.SubElement(record_el, "field", name="habitacion").text = str(r.get("habitacion", ""))
        ET.SubElement(record_el, "field", name="estado").text = str(r.get("estado", ""))

    # facturas 
    for f in facturas:
        record_id = f"hotel_factura_{f.get('id', '')}"
        record_el = ET.SubElement(data_el, "record", id=record_id, model="hotel.factura")
        
        ET.SubElement(record_el, "field", name="reserva_id", ref=f"hotel_reserva_{f.get('reservaId', '')}")
        ET.SubElement(record_el, "field", name="total").text = str(f.get("total", ""))
        ET.SubElement(record_el, "field", name="fecha").text = str(f.get("fecha", ""))

    # guardar XML
    nombre_archivo = f"export_odoo_{nombre_lote}_{datetime.now().strftime('%Y%m%d_%H%M%S')}.xml"
    tree = ET.ElementTree(root)
    
    # Indentación básica (Pretty Print) para que sea legible
    ET.indent(tree, space="    ", level=0)

    ruta_salida = os.path.join(os.getcwd(), nombre_archivo)
    tree.write(ruta_salida, encoding="utf-8", xml_declaration=True)

    print(f"XML generado correctamente: {ruta_salida}")


if __name__ == "__main__":
    try:
        nombre_lote = sys.argv[1] if len(sys.argv) > 1 else "LoteManual"
        
        data = cargar_datos()

        clientes = data.get("clientes", [])
        reservas = data.get("reservas", [])
        facturas = data.get("facturas", [])

        generar_xml(clientes, reservas, facturas, nombre_lote)

    except Exception as e:
        print(f"ERROR: {str(e)}")