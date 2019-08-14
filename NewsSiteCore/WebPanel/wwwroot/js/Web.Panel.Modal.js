/// <reference path="Web.Panel.js" />

Web.Panel.ModalItemTypeEnum = {
    CREATE: 1,
    EDIT: 2,
    DELETE: 3,
    SELECT: 4,
    INFORMATION: 5
};

Web.Panel.Modal.Item = {
    show: function (type, html, confirm) {
        var arg0 = arguments[3];
        var arg1 = arguments[4];
        var arg2 = arguments[5];
        var arg3 = arguments[6];
        var arg4 = arguments[7];

        if (type === Web.Panel.ModalItemTypeEnum.DELETE) {
            $('#model-main').removeClass('info');
            $('#model-main-dialog').removeClass('modal-lg');
            $('#model-main').addClass('danger');
            $('#model-main-confirm').removeClass('btn-info');
            $('#model-main-confirm').addClass('btn-danger');
            $('#model-main-confirm').html('Sil');
        } else if (type === Web.Panel.ModalItemTypeEnum.REPORT) {
            var _width = $(window).width() - 100;
            var _height = $(window).height() - 175;
            $('#model-main-dialog').css("width", _width);
            $('#model-main-dialog').css("height", _height);
            $('#model-main').addClass('danger');
            $('#model-main-confirm').removeClass('btn-primary');
            $('#model-main-confirm').addClass('btn-danger');
        }
        else {
            $('#model-main').removeClass('danger');
            $('#model-main-dialog').addClass('modal-lg');
            $('#model-main').addClass('info');
            $('#model-main-confirm').removeClass('btn-danger');
            $('#model-main-confirm').addClass('btn-info');
            $('#model-main-confirm').html('Kaydet');
        }

        if (type === Web.Panel.ModalItemTypeEnum.CREATE) {
            $('#model-main-title').html('Yeni');
        }
        if (type === Web.Panel.ModalItemTypeEnum.EDIT) {
            $('#model-main-title').html('Düzenle');
        }
        if (type === Web.Panel.ModalItemTypeEnum.DELETE) {
            $('#model-main-title').html('Sil');
        }
        if (type === Web.Panel.ModalItemTypeEnum.SELECT) {
            $('#model-main-title').html('Seç');
            $('#model-main-confirm').hide();
        }
        if (type === Web.Panel.ModalItemTypeEnum.INFORMATION) {
            $('#model-main-title').html('Information');
            $('#model-main-confirm').html('OK!');
            $('#model-main-cancel').hide();
        }

        $('#model-main-body').html(html);
        $('#model-main-confirm').off('click');
        $('#model-main-confirm').on('click', function () {
            if (typeof confirm === 'function') {
                confirm(arg0, arg1, arg2, arg3, arg4);
            }
        });
        $('#model-main').modal({ backdrop: 'static' });

        //$('input.icheck').iCheck({
        //    checkboxClass: 'icheckbox_square-blue checkbox',
        //    radioClass: 'iradio_square-blue'
        //});
        //$(".datetime").datetimepicker({ autoclose: true });
        //$('input.switch').bootstrapSwitch();
        //$('.bootstrap-switch-handle-on').text($('.switch').data('label-on'));
        //$('.bootstrap-switch-handle-off').text($('.switch').data('label-off'));
        //$(".tags").select2({ tags: 0, width: '100%' });
        //$('.bslider').slider();
        $("#report-iframe").width($('#model-main-dialog').width() - 50);
        $("#report-iframe").height($('#model-main-dialog').height() - 80);
    },
    hide: function () {
        $('#model-main').modal('hide');
    }
};
