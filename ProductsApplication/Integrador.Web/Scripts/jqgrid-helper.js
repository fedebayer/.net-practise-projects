/*Importante, reajustado para grilla de lista de retenciones, idea para grillas etapa 2
ya que se implementara seleccion multiple en varias pantallas.
aprovechar propiedad multiselect.

Implementar columna "Info" en el columnModel en todas las grillas visbles

Menu izquierdo -> Documentos - > Todas las pantallas.
Estado de cuenta -> Todas las pantallas.    
*/

function jqgridDefault( gridID, gridPagerID, gridCaption, gridColumNames, gridColumnModel, onErrorHandler ) {
    
    $( "#" + gridID ).jqGrid( {
        mtype: "GET",
        datatype: "local",
        caption: gridCaption,
        pager: "#" + gridPagerID,
        colNames: gridColumNames,
        colModel: gridColumnModel,
        editable: false,
        formatter: 'actions',
        formatoptions: { keys: true,
            editformbutton: true
        },
        loadError: function ( jqXHR, textStatus, errorThrown ) { if ( onErrorHandler != null ) { onErrorHandler( jqXHR, textStatus, errorThrown ); } },
        autowidth: true,
        //shrinkToFit: false,        
        height: 'auto',
        hidegrid: false,
        rowNum: 10,
        viewrecords: true,
        rowList: [10, 20, 30],
        multiselect: gridColumnModel[0].isInfoModel == true ? gridColumnModel[0].multiselect : false
        //onPaging: function (type) {

        //$('#' + gridID).parents("div.ui-jqgrid-bdiv").css("max-height", "500px");

        //$('#' + gridID).css({ width: "" });

        //$('#' + gridID).css({ width: jQuery(".ui-jqgrid").parent().width() - 50 });

        //$('.ui-jqgrid-bdiv').css({ width: jQuery(".ui-jqgrid").parent().width()+0.5 });

        //},        
    } ).jqGrid( 'navGrid', gridPagerID, { edit: false, add: false, del: false } );

    // recalcular ancho
    //$('.ui-jqgrid-bdiv').css({ width: jQuery(".ui-jqgrid").parent().width()+0.5 });
    //$('#' + gridID).css({ width: "" });

    // alto maximo para que genere scroll vertical.
    $( '#' + gridID ).parents( "div.ui-jqgrid-bdiv" ).css( "max-height", "270px" );


    // On Resize
    $( window ).resize( function ()
    {

        if ( window.afterResize )
        {
            clearTimeout( window.afterResize );
        }

        window.afterResize = setTimeout( autowidthGrid, 1000 );
    } );
    
    function calculateWidth()//Solo para mobiles
    {//Realizo un promedio de pixeles por columna, quitando las 3 por default que están ocultas en todas las grillas.
     //Revisar, implementar columna Info en columnModel. Aplicado en Administracion/lista-retenciones.js
        var info = gridColumnModel[0].isInfoModel == true ? gridColumnModel[0] : 0;

        var gridWidth = $(".ui-jqgrid").parent().width();
        var cantidadColumnasReales = gridColumNames.length - 3;
        
        var calculo;

        var widthAdecuado;

        if (info != 0) {

            widthAdecuado = info.columWidtTotal;

        }
        //Quitar else if y else una vez implementado columna "Info" en el columnModel.
        else if ( cantidadColumnasReales < 7 )
        {
            calculo = (gridColumNames.length - 3) * 93; /*Promedio de 93 pixeles por columna*/
            widthAdecuado = gridWidth > calculo ? gridWidth : calculo;
        } else
        {
            calculo = (gridColumNames.length - 3) * 100; /*Promedio de 100 pixeles por columna*/
            widthAdecuado = gridWidth > calculo ? gridWidth : calculo;
        }

        return widthAdecuado;
    }

    function autowidthGrid( colmodel )
    {
        var esMobile = /Android|webOS|iPhone|iPod|BlackBerry|IEMobile|Opera Mini/i.test( navigator.userAgent ); //Mobiles omitiendo el Ipad ya que tiene una resolución muy grande
        if (esMobile && window.innerWidth < 768)
        {
            $('.ui-jqgrid-view').css({ width: $( ".ui-jqgrid" ).parent().width(), 'overflow': 'auto' } ); //aplico overflow a la grilla (contenedor de header y body)

            $('.ui-jqgrid-pager').css({ width: $( ".ui-jqgrid" ).parent().width() + 'px' } ); //Width del paginador

            $( '.ui-paging-info' ).css( 'display', 'none' ); //En mobiles oculto cantidad de registros obtenidos hasta ordenar,( hasta ahora se superponian los divs del paginador. )

            $( '.ui-jqgrid-hdiv' ).css( { width: calculateWidth() + 'px' } ); //Width del header

            $( '.ui-jqgrid-bdiv' ).css( { width: calculateWidth() + 'px' } ); //Width del body

            $( '.ui-jqgrid-bdiv' ).css( 'overflow-x', 'hidden' ); //Oculto el overflow del body para que no se duplique con el de la grilla

        } else
        {
            $("#" + gridID).jqGrid('setGridWidth', $( ".ui-jqgrid" ).parent().width() );

            $('.ui-jqgrid-view').css({ width: $( ".ui-jqgrid" ).parent().width(), 'overflow': 'auto' } );

            $('.ui-jqgrid-bdiv').css({ width: $( ".ui-jqgrid" ).parent().width() } );
            
            if ( window.innerWidth < 768 )//Cuando es web y achican la ventana
            {
                $( '.ui-paging-info' ).css( 'display', 'none' );

            } else
            {
                $( '.ui-paging-info' ).css( 'display', 'block' );
            }
        }

    }

    autowidthGrid( gridColumnModel );//Llamo al autowidth para la primer carga de la pantalla.
}

function jqgridLoadData( gridID, data )
{

    $( '#' + gridID ).jqGrid( "clearGridData" );

    $( '#' + gridID ).jqGrid( 'setGridParam', { 'data': data } )
                                        .trigger( 'reloadGrid' );
}

