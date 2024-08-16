/*FB: Se encapsula en una funcion la parte de limpiar campos que se repite el codigo como en las funciones btNuevoClick, aceptar y cancelar.
 *      de esta forma se reutiliza codigo, ahoramos varias lineas repetidas, y dividimos la solucion en partes mas pequeñas
 *    Se pasa a una constante las url producto que dirigen controller para facilitar futuros cambios/mantenimiento
 *    Se cambian y se ponen en constantes las variables de tipo N, U Y D a New, Update y Delete para ser mas representativo y evitar confusion
 *    Se arregla error de cartel vacio en notificacion luego de eliminar exitosamente un producto, estaba 'data.content.mensaje' pero .mensaje no existe
 *      se dejo en 'data.content' y ahora funciona correctamente mostrando el mensaje en la notificacion
 *    Se valida que se haya ingresado todos los campos en la funcion btnAceptar antes de ingresar un producto nuevo*/


const NEW = 'New';
const UPDATE = 'Update';
const DELETE = 'Delete';

$(document).ready(function () {
    $("#divSearch").show();
    $("#divNewUpdate").hide();
    $('#btnExportar').hide();

    cargarHandlers();
    inicializarGrilla();
    btnBuscarClick();
});

function cargarHandlers() {
    $('#btnBuscar').click(btnBuscarClick);
    $('#btnLimpiarFiltros').click(btnLimpiarFiltrosClick);
    $('#btnNuevo').click(btnNuevoClick);
    $('#btnCancelar').click(btnCancelarClick);
    $('#btnAceptar').click(btnAceptarClick);
}

function btnNuevoClick() {
    limpiarCampos()

    $("#divSearch").hide();
    $("#divNewUpdate").show();
    $("#hdnTipo").val(NEW);

    $("#lblNuevo").show();
    $("#lblModificar").hide();
}

function btnCancelarClick() {
    limpiarCampos()

    $("#divNewUpdate").hide();
    $("#divSearch").show();
}

function limpiarCampos() {
    $("#hdnTipo, #txtDescripcion, #txtCategoria, #txtDescripcion_NU, #intCategoria_NU, #hiddenId").val('');
}

const gestionProductoURL = '/Producto/GestionProducto';
const buscarProductoURL = '/Producto/BuscarProducto';





function btnAceptarClick() {
    var tipo = $("#hdnTipo").val();
    var descripcion = $("#txtDescripcion_NU").val();
    var IdCategoria = $("#intCategoria_NU").val();
    var id = $("#hiddenId").val();


    if (!descripcion || !IdCategoria) {
        NotificationUi.CrearNotificationBoxFor($('#notificationBox'), "alert-info", "Debe completa todos los campos para crear nuevo producto", true);
        return;
    }

    if (descripcion.length > 100) {
        NotificationUi.CrearNotificationBoxFor($('#notificationBox'), "alert-info", "La descripción debe ser menor a 100 caracteres", true);
        return;
    }
    

    $.ajax({
        type: "POST",
        url: gestionProductoURL,
        data: { tipo, descripcion, IdCategoria, id },
        success: function (data) {

            if (data.result == 0) {
                NotificationUi.CrearNotificationBoxFor($('#notificationBox'), "alert-success", data.content, true);

                limpiarCampos()

                $("#divNewUpdate").hide();
                $("#divSearch").show();

                btnBuscarClick();
            }
            else {
                NotificationUi.CrearNotificationBoxFor($('#notificationBox'), "alert-danger", data.content, true);
            }
        },
        error: function (data) {
            NotificationUi.CrearNotificationBoxFor($('#notificationBox'), "alert-danger", "Error de conexión", true);
        }
    });
}

function btnLimpiarFiltrosClick() {
    $('#txtDescripcion').val('');
    $('#txtCategoria').val('');
    btnBuscarClick();
}


function btnBuscarClick() {
    var searchUrl = buscarProductoURL + "?Descripcion=" + $('#txtDescripcion').val() + "&Categoria=" + $('#txtCategoria').val();

    $('#grilla').jqGrid('setGridParam',
        {
            'url': searchUrl,
            'datatype': 'json',
            'page': 1
        })
        .trigger('reloadGrid');
}

function inicializarGrilla() {

    var columns = ['ID', 'IdCategoria', 'Descripcion', 'Precio', 'Categoria', 'Acciones'];

    var columnModel = [{ name: 'ID', index: 'ID', hidden: true },
    { name: 'IdCategoria', index: 'IdCategoria', hidden: true },
    { name: 'Descripcion', index: 'Descripcion', resizable: false, fixed: true, align: 'center', width: '275%' },
    { name: 'Precio', index: 'Precio', resizable: false, fixed: true, align: 'center', width: '275%' },
    { name: 'DescripcionCategoria', index: 'DescripcionCategoria', resizable: false, fixed: true, align: 'center', width: '275%' },
    { align: "center", editable: false, sortable: false, resizable: false, fixed: true, align: 'center', width: '150%', formatter: botonFormatter }];

    jqgridDefault("grilla",
        "grillaPaginador",
        '',
        columns,
        columnModel,
        function (jqXHR, textStatus, errorThrown) {
            NotificationUi.CrearNotificationBoxFor($('#notificationBox'), "alert-danger", "Error de conexión", true);
        });
}

function botonFormatter(cellvalue, options, rowObject) {

    var row = "<div>";

    var handlerUpdateCallString = "javascript:handlerUpdate('" + encodeURIComponent(rowObject.Descripcion) + "," + encodeURIComponent(rowObject.IdCategoria) + "," + rowObject.ID + "')";
    botonUpdateString = "<a title='Modificar' href= " + handlerUpdateCallString + "><span class='glyphicon glyphicon-edit'></span></a>";

    row += botonUpdateString;

    var handlerDeleteCallString = "javascript:handlerDelete('" + rowObject.ID + "')";
    botonDeleteString = "<a title='Eliminar' href= " + handlerDeleteCallString + "><span class='glyphicon glyphicon-remove-sign'></span></a>";

    row += botonDeleteString;

    row += "</div>";

    return row;
}

function handlerUpdate(valor) {
    var valores = valor.split(",");
    var descripcion = valores[0]
    var categoria = valores[1]
    var id = valores[2]
    $("#hdnTipo").val(UPDATE);
    $("#txtDescripcion_NU").val(descripcion);
    $("#intCategoria_NU").val(categoria);
    $("#hiddenId").val(id);
    $("#divSearch").hide();
    $("#divNewUpdate").show();

    $("#lblNuevo").hide();
    $("#lblModificar").show();
}

function handlerDelete(valor) {
    $.ajax({
        type: "POST",
        url: gestionProductoURL,
        data: { tipo: DELETE, descripcion: "", IdCategoria: 0, id: valor },
        success: function (data) {

            if (data.result == 0) {
                btnBuscarClick();

                NotificationUi.CrearNotificationBoxFor($('#notificationBox'), "alert-success", data.content, true);
            }
            else {
                NotificationUi.CrearNotificationBoxFor($('#notificationBox'), "alert-danger", data.content, true);
            }
        },
        error: function (data) {
            NotificationUi.CrearNotificationBoxFor($('#notificationBox'), "alert-danger", "Error de conexión", true);
        }
    });
}