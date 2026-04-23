import xml.etree.ElementTree as ET
import json
from datetime import datetime
import os


def cargar_datos():
    """Carga el JSON generado por el backend"""
    ruta = os.path.join(os.getcwd(), "data_export.json")

    if not os.path.exists(ruta):
        raise FileNotFoundError("No se encontró data_export.json")

    with open(ruta, "r", encoding="utf-8") as f:
        return json.load(f)


def generar_xml(clientes, reservas, facturas):
    root = ET.Element("HotelSOLExport")

    # clientes
    clientes_el = ET.SubElement(root, "Clientes")
    for c in clientes:
        cliente_el = ET.SubElement(clientes_el, "Cliente", id=str(c.get("id", "")))

        ET.SubElement(cliente_el, "Nombre").text = str(c.get("nombre", ""))
        ET.SubElement(cliente_el, "Documento").text = str(c.get("documento", ""))
        ET.SubElement(cliente_el, "Telefono").text = str(c.get("telefono", ""))
        ET.SubElement(cliente_el, "VIP").text = str(c.get("vip", False)).lower()

    # reservas
    reservas_el = ET.SubElement(root, "Reservas")
    for r in reservas:
        reserva_el = ET.SubElement(reservas_el, "Reserva", id=str(r.get("id", "")))

        ET.SubElement(reserva_el, "ClienteId").text = str(r.get("clienteId", ""))
        ET.SubElement(reserva_el, "FechaEntrada").text = str(r.get("fechaEntrada", ""))
        ET.SubElement(reserva_el, "FechaSalida").text = str(r.get("fechaSalida", ""))
        ET.SubElement(reserva_el, "Habitacion").text = str(r.get("habitacion", ""))
        ET.SubElement(reserva_el, "Estado").text = str(r.get("estado", ""))

    # facturas
    facturas_el = ET.SubElement(root, "Facturas")
    for f in facturas:
        factura_el = ET.SubElement(facturas_el, "Factura", id=str(f.get("id", "")))

        ET.SubElement(factura_el, "ReservaId").text = str(f.get("reservaId", ""))
        ET.SubElement(factura_el, "Total").text = str(f.get("total", ""))
        ET.SubElement(factura_el, "Fecha").text = str(f.get("fecha", ""))

    # guardar XML
    nombre_archivo = f"export_odoo_{datetime.now().strftime('%Y%m%d_%H%M%S')}.xml"
    tree = ET.ElementTree(root)

    ruta_salida = os.path.join(os.getcwd(), nombre_archivo)
    tree.write(ruta_salida, encoding="utf-8", xml_declaration=True)

    print(f"XML generado correctamente: {ruta_salida}")


if __name__ == "__main__":
    try:
        data = cargar_datos()

        clientes = data.get("clientes", [])
        reservas = data.get("reservas", [])
        facturas = data.get("facturas", [])

        generar_xml(clientes, reservas, facturas)

    except Exception as e:
        print(f"ERROR: {str(e)}")