// v2.2.4
// v2.2.5 detail select2
// v2.2.6 bug
// v2.2.7 bug todatestring
// v2.2.8 other ajax
// v2.3.0 bug
// v2.3.1 isetup stateDataOnSuccess
// v2.3.2 itable delete
// v2.3.3 select2 data & onchange input type file
// v2.3.4 isGetData
// v2.3.5 bug formula
// v2.4.5 isetup update
// v2.4.6 setFormFromModel
// v2.4.7 idGenerator
// v2.5.0 iPainter
// v2.5.1 isValid
// v2.5.2 initDetailTr
// v2.5.3 formula
// v2.5.4 stateDataOnSuccess return false
// v2.5.5 select2 OtherParam isetup.select2OtherParam
// v2.5.6 validURL iPainter.getSrc
// v2.5.7 bug iPainter.getSrc
// v2.5.8 filterAutoResetPageIndex
// v2.5.9 triggerModels select2 bug
// 2.5.10 isetup init ipainter update
// 2.5.11 initHelper update bug
// 2.6.0 .net core 6

const triggerModels = (isetup, modelKey) => {
    const model = isetup.models[modelKey];
    Object.keys(model).forEach(x => {
        const t = $(isetup.id).find(`[data-model="${modelKey}.${x}"]`);
        if (t.length) {
            if (t.attr('type') === 'checkbox')
                t.is(':checked', model[x]);
            else if (t.attr('type') === 'radio') {
                $.each(t, (i, v2) => {
                    if (v2.value === model[x])
                        v2.checked = true;
                });
            } else if (t.is('select') && t.hasClass('select2-hidden-accessible')) {
                if (model[x]) {
                    t.append(`<option value="${model[x]}">${model[`${x}_Text`]}</option>`);
                }
                t.val(model[x]);
                t.trigger('change');
            } else
                t.val(model[x]);
        }
    });
};

const mahasFormula = (j, isetup) => {
    $.each(j.find('[data-name]:not(.mahas-formula),[name]:not(.mahas-formula)'), (i, v) => {
        var jv = $(v);
        switch (jv.prop('tagName')) {
            case 'INPUT':
                jv.on('change', e => mahasFormulaProcess(e, isetup, !!jv.data('name')));
                break;
            case 'SELECT':
                jv.on('change', e => mahasFormulaProcess(e, isetup, !!jv.data('name')));
                break;
            case 'TEXTAREA':
                jv.on('change', e => mahasFormulaProcess(e, isetup, !!jv.data('name')));
                break;
        }
        jv.addClass('mahas-formula');
    });
    triggerFormula(isetup);
};

const triggerFormula = (isetup) => {
    $.each($(isetup.id).find(`[data-formula]`), (i, v) => {
        const name = $(v).attr('name');
        const formula = $(v).data('formula');
        if (formula.indexOf('javascript:') === 0) {
            mahasFormulaProcessJs($(isetup.id));
        } else {
            (formula.match(/(\@\w+\.\w+)|(\@\w+)/g) || []).forEach(x => {
                const y = x.replace(`@`, '');
                if (name) {
                    $($(v).closest('form')[0][y]).trigger('change');
                } else {
                    $(v).closest('tr').find(`[data-name="${y}"]`).trigger('change');
                }
            });
        }
    });
};

const mahasFormulaProcess = (e, isetup, detail) => {
    if (isetup.isGetData) return;

    const key = typeof e === 'string' ? e : $(e.target).data('name') || e.target.id;
    const form = $(isetup.id).find('form')[0];

    if (detail) {
        const tr = $(e.target).closest('tr');
        const code = tr.closest('.table-detail').data('name');
        $.each(tr.find(`[data-formula*="@${key}"]`), (i, v) => {
            let formula = $(v).data('formula');
            if (formula.indexOf('javascript:') < 0) {
                (formula.match(/(\@\w+\.\w+)|(\@\w+)/g) || []).forEach(x => {
                    const y = x.replace(`@`, '');
                    const inputy = tr.find(`[data-name="${y}"]`);
                    if (inputy.hasClass('mahas-currency')) {
                        formula = formula.replace(x, isetup.cleave[`Detail_${code}_${y}_${JSON.stringify(tr.data('key'))}`].getRawValue());
                    } else if (inputy.attr('type') === 'checkbox' || inputy.attr('type') === 'radio') {
                        formula = formula.replace(x, inputy[0].checked);
                    } else {
                        formula = formula.replace(x, inputy.val());
                    }
                });
            }
            let value;
            try { value = eval(formula) } catch { }
            $(v).val(value);
        });
    }

    $.each($(`[name][data-formula*="@${key}"]`), (i, v) => {
        let formula = $(v).data('formula');
        if (formula.indexOf('javascript:') < 0) {
            (formula.match(/(\@\w+\.\w+)|(\@\w+)/g) || []).forEach(x => {
                const y = x.replace(`@`, '');
                if (form[y]) {
                    if ($(form[y]).hasClass('mahas-currency')) {
                        formula = formula.replace(x, isetup.cleave[y].getRawValue());
                    } else if (form[y].type === 'checkbox' || form[y].type === 'radio') {
                        formula = formula.replace(x, form[y].checked);
                    } else {
                        formula = formula.replace(x, form[y].value);
                    }
                }
            });
        }
        let value;
        try { value = eval(formula) } catch { }
        $(v).val(value);
    });

    mahasFormulaProcessJs($(isetup.id));
};

const mahasFormulaProcessJs = j => {
    $.each(j.find('[data-formula^="javascript:"]'), (i, v) => {
        const formula = $(v).data('formula').replace('javascript:', '');
        let value;
        const oldValue = $(v).val();
        try { value = eval(formula) } catch { }
        $(v).val(value);
        if (value != oldValue) $(v).trigger('change');
    });
};

const isLaravel = () => {
    return typeof(framework) != 'undefined' && framework === 'laravel';
};

class iTable {
    constructor(opt = {}) {
        this.id = '#' + opt.id;
        this.url = opt.url;
        this.urlType = opt.urlType;
        this.success = opt.success;
        this.complete = opt.complete;
        this.beforeSend = opt.beforeSend;
        this.filter = opt.filter;
        this.otherData = opt.otherData;
        this.lookupSelectedItems = [];
        this.lookupSelected = opt.lookupSelected;
        this.property(opt);

        let t = this;

        $(this.id).find('[data-itable-sorting]').on('click', e => {
            let _orderBy = $(e.target).data('itable-sorting');
            let _orderByType = _orderBy === t.orderBy ? !t.orderByType : true;
            t.property({ orderBy: _orderBy, orderByType: _orderByType });
            t.pageIndex = 0;
            t.refresh();
        });

        $(this.id).find('.itable-form').on('submit', e => {
            e.preventDefault();
            t.refresh();
        });

        const tableLookup = $(this.id).find('[class*=table-lookup]');
        if (tableLookup.length) {
            $(this.id).find('.btn-selected').on('click', e => {
                t.prosesLookupSelected();
            });
            $(this.id).on('hidden.bs.modal', e => {
                if ($('.modal.show').length > 0)
                    $('body').addClass('modal-open');
            });
        }
    };

    trempty() {
        return `<tr><td colspan="${$(this.id).find('thead th').length}" class="text-center">----- <b>Tidak ada data</b> -----</td></tr>`;
    };

    delete(param, itable) {
        let t = this;
        modalDialog('warning', '',
            `
                <div class="text-center">
                    <i class="icon-warning22 text-warning display-1"></i>
                    <h3>Are you sure ?</h3>
                    <label>Data will be delete</label>
                </div>
            `
            , () => {
                $.ajax({
                    url: t.urlType["DELETE"] || t.url,
                    type: 'DELETE',
                    data: param,
                    success: r => {
                        if (itable) itable.refresh();
                    },
                    error: xhr => {
                        alertError(xhr);
                    },
                    beforeSend: () => {
                        loading(true);
                    },
                    complete: () => {
                        loading(false);
                    }
                });
            });
    };

    refresh() {
        if (this.isLoading()) return;
        this.pageSize = parseInt(
            $(this.id).find('.itable-pagesize option:selected').val() ||
            $(this.id).find('.itable-pagesize').val() ||
            0
        );
        let _filter = [];
        if (this.filter) {
            _filter = this.filter();
        } else {
            $.each($(this.id).find('[data-itable-filter]'), (i, v) => {
                _filter.push({
                    Key: $(v).data('itable-filter'),
                    Value: v.type === 'checkbox' ? v.checked : v.value
                });
            });
        }
        const t = this;
        const _ajaxData = {
            orderBy: this.orderBy,
            orderByType: this.orderByType ? 'ASC' : 'DESC',
            pageSize: this.pageSize,
            pageIndex: this.pageIndex,
            filter: _filter,
            ...(typeof this.otherData === 'function' ? this.otherData() : this.otherData || {})
        };
        if (isLaravel()) {
            _ajaxData._token = $(t.id).find('form')[0]['_token']?.value;
        } else {
            _ajaxData.__RequestVerificationToken = $(t.id).find('form')[0]['__RequestVerificationToken']?.value;
        }
        $.ajax({
            url: this.url,
            type: 'POST',
            data: _ajaxData,
            success: (r) => {
                if (typeof t.success === 'function') {
                    t.success(r, t);
                    const tableLookup = $(t.id).find('[class*=table-lookup]');
                    if (tableLookup.length) {
                        tableLookup.find('tbody tr').on('click', e => {
                            const tr = $(e.target).closest('tr');
                            if (tableLookup.hasClass('table-lookup-multiple')) {
                                tr.toggleClass('detail-active');
                                if (tr.hasClass('detail-active')) {
                                    t.lookupSelectedItems.push(tr.data('model'));
                                } else {
                                    t.lookupSelectedItems.splice(t.lookupSelectedItems.indexOf(tr.data('model')), 1);
                                }
                            } else {
                                if (tr.hasClass('detail-active')) {
                                    t.prosesLookupSelected();
                                } else {
                                    tr.closest('tbody').find('.detail-active').removeClass('detail-active');
                                    tr.toggleClass('detail-active');
                                    t.lookupSelectedItems = [];
                                    t.lookupSelectedItems.push(tr.data('model'));
                                }
                            }
                            t.lookupChangeButtonText();
                        });
                        $.each(tableLookup.find('tbody tr'), (i, v) => {
                            if (t.lookupSelectedItems.find(x => JSON.stringify(x) === JSON.stringify($(v).data('model'))))
                                $(v).addClass('detail-active');
                        });
                    }
                }
                $(t.id).find('[data-itable-page]').on('click', e => {
                    t.property({ pageIndex: $(e.target).data('itable-page') });
                    t.refresh();
                });
            },
            error: (xhr) => {
                alertError(xhr);
            },
            beforeSend: () => {
                t.loading(true);
                if (typeof t.beforeSend === 'function') {
                    t.beforeSend();
                }
            },
            complete: () => {
                t.loading(false);
                if (typeof t.complete === 'function') {
                    t.complete();
                }
            }
        });
    };

    lookupChangeButtonText() {
        const tag = $(this.id).find('.selected-count');
        if (tag.length) {
            tag.html(
                this.lookupSelectedItems.length == 0 ? '' :
                    `(${this.lookupSelectedItems.length})`
            );
        }
    };

    prosesLookupSelected() {
        if (typeof this.lookupSelected === 'function')
            this.lookupSelected(this.lookupSelectedItems);
        $(this.id).modal('hide');
    };

    property(opt = {}) {
        let t = this;
        t.orderBy = opt.orderBy === undefined ? t.orderBy : opt.orderBy;
        t.orderBy = t.orderBy === undefined ? 0 : t.orderBy;
        let _orderByType = t.orderByType === undefined ? true : t.orderByType;
        t.orderByType = opt.orderByType === undefined ? _orderByType : opt.orderByType;
        let _pageIndex = t.pageIndex === undefined ? 0 : t.pageIndex;
        t.pageIndex = opt.pageIndex === undefined ? _pageIndex : opt.pageIndex;
        $(t.id).find('[data-itable-sorting]')
            .removeClass('sorting')
            .removeClass('sorting_asc')
            .removeClass('sorting_desc');
        $.each($(t.id).find('[data-itable-sorting]'), (i, v) => {
            $(v).addClass($(v).data(
                'itable-sorting') === t.orderBy ?
                ('sorting_' + (t.orderByType ? 'asc' : 'desc'))
                : 'sorting'
            );
        });
    };

    generateTableFooter(totalCount, pageSize, currentPage) {
        let startData;
        if (totalCount === 0) startData = 0;
        else startData = currentPage * pageSize + 1;
        let maxPage = Math.ceil(totalCount / pageSize);
        let endData = startData + pageSize - 1;
        if (endData > totalCount) endData = totalCount;
        let dataInfo = `Showing ${startData} to ${endData} of ${totalCount} entries (Page ${totalCount === 0 ? 0 : currentPage + 1}/${maxPage})`;
        let pagination = "";
        if (totalCount > 0) {
            let startPage = currentPage > 0 ? currentPage - 1 : 0;
            let endPage = currentPage + 1;
            if (currentPage === 0) endPage++;
            endPage = endPage > maxPage - 1 ? maxPage - 1 : endPage;
            if (currentPage >= endPage && startPage > 0) startPage--;

            pagination += `<a ${currentPage != 0 && 'data-itable-page="0"'} class="paginate_button previous ${currentPage === 0 && 'disabled'}">&laquo;</a>`;
            pagination += `<a ${currentPage != 0 && `data-itable-page="${currentPage - 1}"`} class="paginate_button previous ${currentPage === 0 && 'disabled'}">&lsaquo;</a>`;

            pagination += '<span>';
            if (startPage > 0)
                pagination += `<a class="paginate_button" data-itable-page="${startPage - 1}">..</a>`;
            for (let i = startPage; i <= endPage; i++) {
                let active = i === currentPage;
                pagination += `<a class="paginate_button ${active && 'current'}" ${!active && `data-itable-page="${i}"`}>${i + 1}</a>`;
            }
            if (endPage + 1 < maxPage)
                pagination += `<a class="paginate_button" data-itable-page="${endPage + 1}">..</a>`;
            pagination += '</span>';
            pagination += `<a ${currentPage + 1 != maxPage && `data-itable-page="${currentPage + 1}"`} class="paginate_button next ${currentPage + 1 === maxPage && 'disabled'}">&rsaquo;</a>`;
            pagination += `<a ${currentPage + 1 != maxPage && `data-itable-page="${maxPage - 1}"`} class="paginate_button next ${currentPage + 1 === maxPage && 'disabled'}">&raquo;</a>`;
        }

        if ($(this.id).find('.datatable-footer').length === 0) {
            const htmlfooter = `
                            <div class="datatable-footer">
                                <div class="dataTables_info"></div>
                                <div class="dataTables_paginate paging_simple_numbers"></div>
                            </div>
                        `;
            if ($(this.id).hasClass('modal'))
                $(this.id).find('.modal-body').append(htmlfooter);
            else
                $(this.id).append(htmlfooter);
        }
        $(this.id).find('.dataTables_info').html(dataInfo);
        $(this.id).find('.dataTables_paginate').html(pagination);
    };

    isLoading() {
        return $(this.id).find('#itable_loading').length > 0;
    };

    loading(isLoading) {
        if (isLoading) {
            const html = `
                <div id="itable_loading">
                    <i class="icon-spinner2 spinner" style="
                        top: calc(50% - 0.5rem);
                    "></i>
                </div>
            `;
            if ($(this.id).hasClass('modal'))
                $(this.id).find('.modal-content').append(html);
            else
                $(this.id).append(html);
        } else {
            $(this.id).find('#itable_loading').remove();
        }
    };

    openModal() {
        $(this.id).css('z-index', 1060);
        $(this.id).modal({ backdrop: 'static' });
        $('.modal-backdrop.fade.show').last().css('z-index', $(this.id).css('z-index') - 1);
        this.lookupSelectedItems = [];
        this.lookupChangeButtonText();
        this.pageIndex = 0;
        this.refresh();
    };
}

class iSetup {
    constructor(opt = {}) {
        this.id = "#" + opt.id;
        this.url = opt.url;
        this.urlType = opt.urlType || [];
        this.param = opt.param;
        this.paramType = opt.paramType || [];
        this.success = opt.success;
        this.complete = opt.complete;
        this.beforeSend = opt.beforeSend;
        this.modalOnReady = opt.modalOnReady;
        this.modalOnClose = opt.modalOnClose;
        this.modalDataOnSuccess = opt.modalDataOnSuccess;
        this.stateDataOnSuccess = opt.stateDataOnSuccess;
        this.dataOnSuccess = opt.dataOnSuccess;
        this.type = this.type || 'POST';
        this.cleave = [];
        this.ipainter = [];
        this.models = [];
        this.otherAjax = opt.otherAjax;
        this.form = $(this.id).find('form');
        this.isGetData = false;
        this.isValid = opt.isValid;
        this.select2OtherParam = opt.select2OtherParam;

        $(this.form).data('isetup', this);
        const t = this;

        $.each($(t.id).find(".mahas-currency"), (i, v) => {
            if (v.name) {
                t.cleave[v.name] = new Cleave(v, { numeral: true, numeralThousandsGroupStyle: 'thousand' });
            }
        });

        $.each($(t.id).find(".ipainter"), (i, v) => {
            if (v.name) {
                $(v).data('src', $(v).attr('src'));
                t.ipainter[v.name] = new iPainter($(v));
            }
        });

        $.each($(t.id).find("select[data-datasource]"), (i, v) => {
            t.select2(v.name, $(v).data('datasource'));
        });

        $.each($(t.id).find(".mahas-autoid"), (i, v) => {
            const jv = $(v);
            jv.on('click', () => {
                $.ajax({
                    url: jv.data('url'),
                    type: 'GET',
                    success: r => {
                        $(v).closest('.form-group').find('input[type="text"]').val(r.id);
                    },
                    error: xhr => {
                        alertError(xhr);
                    },
                    beforeSend: () => {
                        jv.attr('disabled', 'disabled');
                    },
                    complete: () => {
                        jv.removeAttr('disabled');
                    }
                });
            });
        });

        $(this.id).on('shown.bs.modal', e => {
            if (typeof t.modalOnReady === 'function') t.modalOnReady(e, t);
        });

        $(this.id).on('hidden.bs.modal', e => {
            if (typeof t.modalOnClose === 'function') t.modalOnClose(e, t);
        });

        $(this.id).find('form').on('submit', e => {
            e.preventDefault();
            if (isLoading()) return;

            if (typeof t.isValid === 'function') {
                if (!this.isValid(this)) return
            }

            $.ajax({
                url: t.urlType[t.type] || t.url,
                type: t.type,
                data:
                    typeof t.paramType[t.type] === 'function' ? t.paramType[t.type](e)
                        : typeof t.param === 'function' ? t.param(e)
                            : t.param,
                success: r => {
                    if (typeof t.success === 'function') {
                        t.success(r, t);
                    }
                },
                error: xhr => {
                    alertError(xhr);
                },
                beforeSend: () => {
                    loading(true);
                    if (typeof t.beforeSend === 'function') {
                        t.beforeSend();
                    }
                },
                complete: () => {
                    loading(false);
                    if (typeof t.complete === 'function') {
                        t.complete();
                    }
                },
                ...(typeof t.otherAjax === 'function' ? t.otherAjax(t) : t.otherAjax)
            });
        });
    };

    openModal(type, param) {
        let t = this;
        t.type = type;
        const tid = $(t.id);
        tid.find('[class*=show-only-type-]').hide();
        tid.find(`[class*=show-only-type-${type.toLowerCase()}]`).show();
        tid.find('.itable-title-type').html(
            type === 'POST' ? 'New&nbsp;' :
                type === 'PUT' ? 'Edit&nbsp;' :
                    '');

        if (tid.find('form').length > 0) {
            tid.find('form')[0].reset();
            tid.find('.table-detail tbody').html('');
            tid.find('form .open-reset').val('');
            tid.find('form [type="hidden"]:not([name="__RequestVerificationToken"],[name="_token"])').val('');
        }
        t.init();

        if (type === "POST") {
            tid.find('.mahas-autoid').trigger('click');
            tid.modal({ backdrop: 'static' });
        } else if (type === "PUT" || type === "GET") {
            $.ajax({
                url: t.urlType["GET"] || t.url,
                type: 'GET',
                data: param,
                success: r => {
                    if (typeof t.modalDataOnSuccess === 'function') {
                        t.modalDataOnSuccess(r, t);
                        t.init();
                        $(t.id).modal({ backdrop: 'static' });
                    }
                    t.isGetData = false;
                },
                error: xhr => {
                    alertError(xhr);
                    t.isGetData = false;
                },
                beforeSend: () => {
                    loading(true);
                    t.isGetData = true;
                },
                complete: () => {
                    loading(false);
                }
            });
        }
    }

    openState(type, param) {
        let t = this;
        t.type = type;
        const tid = $(t.id);
        tid.find('[class*=show-only-type-]').hide();
        tid.find(`[class*=show-only-type-${type.toLowerCase()}]`).show();
        tid.find('.itable-title-type').html(
            type === 'POST' ? 'New&nbsp;' :
                type === 'PUT' ? 'Edit&nbsp;' :
                    '');

        if (tid.find('form').length > 0) {
            tid.find('form')[0].reset();
            tid.find('.table-detail tbody').html('');
            tid.find('form .open-reset').val('');
            tid.find('form [type="hidden"]:not([name="__RequestVerificationToken"],[name="_token"])').val('');
        }
        t.init();

        if (type === "POST") {
            tid.find('.mahas-autoid').trigger('click');
        } else if (type === "PUT" || type === "GET") {
            $.ajax({
                url: t.urlType["GET"] || t.url,
                type: 'GET',
                data: param,
                success: r => {
                    if (typeof t.stateDataOnSuccess === 'function') {
                        if (t.stateDataOnSuccess(r, t) != false) {
                            t.init();
                        }
                    }
                    t.isGetData = false;
                },
                error: xhr => {
                    alertError(xhr);
                    t.isGetData = false;
                },
                beforeSend: () => {
                    loading(true);
                    t.isGetData = true;
                },
                complete: () => {
                    loading(false);
                }
            });
        }
    }

    delete(param, itable) {
        let t = this;
        modalDialog('warning', '',
            `
                <div class="text-center">
                    <i class="icon-warning22 text-warning display-1"></i>
                    <h3>Are you sure ?</h3>
                    <label>Data will be delete</label>
                </div>
            `
            , () => {
                $.ajax({
                    url: t.urlType["DELETE"] || t.url,
                    type: 'DELETE',
                    data: param,
                    success: r => {
                        if (itable) itable.refresh();
                    },
                    error: xhr => {
                        alertError(xhr);
                    },
                    beforeSend: () => {
                        loading(true);
                    },
                    complete: () => {
                        loading(false);
                    }
                });
            });
    };

    select2(name, url, param, otherParam) {
        const d = $(this.id).find(`[name="${name}"]`);
        mahasSelect2(d, url, param, otherParam);
    };

    details(param, complete) {
        $.ajax({
            url: this.urlType["GET"] || this.url,
            type: 'GET',
            data: param,
            success: r => {
                if (typeof this.dataOnSuccess === 'function') {
                    this.dataOnSuccess(r, this);
                    this.init();
                }
            },
            error: xhr => {
                alertError(xhr);
            },
            beforeSend: () => {
                loading(true);
            },
            complete: () => {
                loading(false);
                if (typeof complete === 'function')
                    complete();
            }
        });
    };

    setDefaultValue() {
        $.each($(this.id).find('[data-defaultvalue]'), (i, v) => {
            const dval = $(v).data('defaultvalue');
            if (v.nodeName === 'SELECT') {
                if (jsonStringIsValid(dval)) {
                    const jval = JSON.parse(dval);
                    if ($(v).find(`option[value="${jval.Value}"]`).length === 0) {
                        $(v).append(`<option value="${jval.Value}">${jval.Text}</option>`);
                    }
                    v.value = jval.Value;
                    $(v).trigger('change');
                }
            } else if (v.nodeName === 'CHECKBOX' || v.nodeName === 'RADIO') {
                v.checked = dval;
            } else {
                v.value = dval;
            }
        });
    };

    init() {
        initHelper($(this.id), this);
    };
}

class iPainter {

    constructor(img, opt = {}) {
        img = typeof img === 'string' ? $(img) : $(img);
        const t = this;
        t.img = img;
        if (opt.temp) return;
        t.colour = 'black';
        t.reset();
        img.css('cursor', 'pointer');
        img.on('click', () => {
            let i = t.initCanvas(opt);
            i = t.drawImage(img, i);
            t.event(i);
            t.eventButton(i, img);
        });
    }

    getSrc = () => {
        const img = document.createElement("img");
        img.src = this.img[0].src;
        img.crossOrigin = "anonymous";

        //var myPromise = new Promise(resolve => {
        //    img.addEventListener('load', () => {
        //        const canvas = document.createElement("canvas");
        //        canvas.width = img.width;
        //        canvas.height = img.height;
        //        const ctx = canvas.getContext("2d");
        //        ctx.drawImage(img, 0, 0);
        //        var dataURL = canvas.toDataURL();
        //        resolve(dataURL);
        //    });
        //});
        //return myPromise;

        const canvas = document.createElement("canvas");
        canvas.width = img.width;
        canvas.height = img.height;
        const ctx = canvas.getContext("2d");
        ctx.drawImage(img, 0, 0);
        var dataURL = canvas.toDataURL();
        return dataURL
    };

    reset = () => {
        this.img.attr('src', this.img.data('src'));
    };

    initCanvas = opt => {
        const id = idGenerator();
        const html = `
                <div id="${id}" class="modal-canvas" style="display:none;">
                    <canvas></canvas>
                    <div class="menu">
                        <div class="row">

                            <div class="col-lg-4 offset-lg-1 col-md-5">
                                <div class="row mb-2 mb-md-0">
                                    <div class="col-4 col-md-5">
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <div class="input-group-text"><i class="icon-font-size"></i></div>
                                            </div>
                                            <input type="number" min="1" max="100" class="form-control ipainter-fontSize" value="${opt.fontSize || '11'}">
                                        </div>
                                    </div>
                                    <div class="col-8 col-md-7">
                                        <div class="input-group">
                                            <input type="text" min="1" max="100" class="form-control ipainter-text" placeholder="Text" value="${opt.text || '11'}">
                                            <div class="input-group-prepend">
                                                <button type="button" class="btn btn-teal ipainter-text-button"><i class="icon-text-color"></i></button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-5">
                                <div class="row mb-2 mb-md-0">
                                    <div class="col-4 col-md-5">
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <div class="input-group-text"><i class="icon-quill2"></i></div>
                                            </div>
                                            <input type="number" min="1" max="100" class="form-control ipainter-lineWidth" value="${opt.lineWidth || '5'}">
                                        </div>
                                    </div>
                                    <div class="col-8 col-md-7">
                                        <button type="button" data-color="black" class="btn ipainter-color bg-grey-800 btn-icon rounded-round mr-2"><i class="icon-droplet"></i></button>
                                        <button type="button" data-color="red" class="btn ipainter-color bg-danger-800 btn-icon rounded-round mr-2"><i class="icon-droplet"></i></button>
                                        <button type="button" class="btn ipainter-eraser bg-teal btn-icon rounded-round mr-2"><i class="icon-eraser"></i></button>
                                    </div>
                                </div>
                            </div>

                            <div class="col-xs-12">
                                <div class="text-right text-md-left">
                                    <button type="button" class="btn btn-default mr-2 ipainter-close">Close</button>
                                    <button type="button" class="btn btn-primary ipainter-ok">OK</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            `;
        $('body').append(html);
        $(`#${id}`).show();
        $('body').addClass('modal-open');

        return {
            id
        };
    };

    drawImage = (img, i) => {
        const canvas = $(`#${i.id} canvas`);
        const context = canvas[0].getContext("2d");
        const image = img[0];
        let h = window.innerHeight - (parseFloat($(`#${i.id} .menu`).css('height')) + 50);
        let w = (image.width / (image.height / h));
        if (w > window.innerWidth) {
            w = window.innerWidth;
            h = (image.height / (image.width / w));
        }
        context.canvas.width = w;
        context.canvas.height = h;
        context.drawImage(image, 0, 0, w, h);

        return {
            canvas,
            context,
            id: i.id
        };
    };

    event = i => {
        const t = this;

        window.requestAnimFrame = (function (callback) {
            return window.requestAnimationFrame ||
                window.webkitRequestAnimationFrame ||
                window.mozRequestAnimationFrame ||
                window.oRequestAnimationFrame ||
                window.msRequestAnimaitonFrame ||
                function (callback) {
                    window.setTimeout(callback, 1000 / 60);
                };
        })();

        var canvas = i.canvas[0];
        var ctx = canvas.getContext("2d");

        let drawingText = false;
        let drawingTextDelay = false;
        let drawing = false;
        let mousePos = { x: 0, y: 0 };
        let lastPos = mousePos;
        canvas.addEventListener("mousedown", e => {
            drawing = true;
            lastPos = getMousePos(canvas, e);
        }, false);
        canvas.addEventListener("mouseup", e => {
            drawing = false;
        }, false);
        canvas.addEventListener("mousemove", e => {
            mousePos = getMousePos(canvas, e);
        }, false);

        canvas.addEventListener("touchstart", e => {
            mousePos = getTouchPos(canvas, e);
            var touch = e.touches[0];
            var mouseEvent = new MouseEvent("mousedown", {
                clientX: touch.clientX,
                clientY: touch.clientY
            });
            canvas.dispatchEvent(mouseEvent);
        }, false);
        canvas.addEventListener("touchend", e => {
            var mouseEvent = new MouseEvent("mouseup", {});
            canvas.dispatchEvent(mouseEvent);
        }, false);
        canvas.addEventListener("touchmove", e => {
            var touch = e.touches[0];
            var mouseEvent = new MouseEvent("mousemove", {
                clientX: touch.clientX,
                clientY: touch.clientY
            });
            canvas.dispatchEvent(mouseEvent);
        }, false);

        const getMousePos = (canvasDom, mouseEvent) => {
            var rect = canvasDom.getBoundingClientRect();
            return {
                x: mouseEvent.clientX - rect.left,
                y: mouseEvent.clientY - rect.top
            };
        }

        const getTouchPos = (canvasDom, touchEvent) => {
            var rect = canvasDom.getBoundingClientRect();
            return {
                x: touchEvent.touches[0].clientX - rect.left,
                y: touchEvent.touches[0].clientY - rect.top
            };
        }

        const renderCanvas = () => {
            if (drawing) {
                ctx.beginPath();

                if (drawingText) {
                    if (drawingTextDelay) {
                        ctx.fillStyle = t.colour;
                        ctx.font = ($('#' + i.id).find('.ipainter-fontSize').val() || '11') + `px Century`;
                        ctx.fillText($('#' + i.id).find('.ipainter-text').val(), lastPos.x, lastPos.y);
                        drawingTextDelay = false;
                    }
                    setTimeout(() => { drawingText = false; }, 500);
                } else {
                    ctx.moveTo(lastPos.x, lastPos.y);
                    ctx.lineTo(mousePos.x, mousePos.y);
                    ctx.strokeStyle = t.colour;
                    ctx.lineWidth = parseFloat($('#' + i.id).find('.ipainter-lineWidth').val()) || 5;
                    ctx.lineCap = 'round';
                }

                ctx.stroke();
                lastPos = mousePos;
            }
        }

        const clearCanvas = () => {
            canvas.width = canvas.width;
        }

        (function drawLoop() {
            requestAnimFrame(drawLoop);
            renderCanvas();
        })();

        $('#' + i.id).find('.ipainter-text-button').on('click', () => {
            drawingText = true;
            drawingTextDelay = true;
        });
    };

    eventButton = (i, img) => {
        const t = this;

        $('#' + i.id).find('.ipainter-eraser').on('click', e => {
            t.drawImage(img, i);
        });

        $('#' + i.id).find('.ipainter-color').on('click', e => {
            this.colour = $(e.target).closest('button').data('color');
        });

        $('#' + i.id).find('.ipainter-close').on('click', () => {
            $('#' + i.id).remove();

            //if ($('.modal.fade.show').length === 0) {
            //    $('body').removeClass('modal-open');
            //}

        });

        $('#' + i.id).find('.ipainter-ok').on('click', e => {
            var dataUrl = i.canvas[0].toDataURL();
            img.attr("src", dataUrl);
            $('#' + i.id).remove();

            //if ($('.modal.fade.show').length === 0) {
            //    $('body').removeClass('modal-open');
            //}

        });
    };
};

const initHelper = (j, isetup, freezerFormula) => {
    const inputreadonly = '.form-control';
    const inputdisabled = 'select,input[type="checkbox"],input[type="radio"],input[type="file"]';
    j.find(inputreadonly).removeAttr('readonly');
    j.find(inputdisabled).removeAttr('disabled');
    j.find('[class*=readonly-only-type-]').removeAttr('readonly');
    j.find(`[class*=readonly-only-type-${isetup.type.toLowerCase()}]`).attr('readonly', 'readonly');
    j.find('[class*=disabled-only-type-]').removeAttr('disabled');
    j.find(`[class*=disabled-only-type-${isetup.type.toLowerCase()}]`).attr('disabled', 'disabled');
    if (isetup.type === 'GET') {
        j.find(inputreadonly).attr('readonly', 'readonly');
        j.find(inputdisabled).attr('disabled', 'disabled');
    } else if (isetup.type === 'POST') {
        j.find('.ipainter').toArray().map(x => $(x).attr('name')).forEach(x => {
            if (isetup.ipainter[x])
                isetup.ipainter[x].reset();
        });
    }
    j.find('select').trigger('change');
    mahasFormula(j, isetup);
};

const mahasSelect2 = (t, url, param, otherParam) => {
    const m = {
        ajax: {
            url: url,
            dataType: 'json', type: "POST", delay: 250,
            data: p => {
                const _param = typeof param === 'function' ? param() : param;
                const _ajaxData = {
                    pageSize: 30,
                    pageIndex: p.page || 0,
                    filter: p.term,
                    ..._param
                };
                const _form = $(t).closest('form')[0];
                if (_form) {
                    if (isLaravel()) {
                        _ajaxData._token = _form['_token']?.value;
                    } else {
                        _ajaxData.__RequestVerificationToken = _form['__RequestVerificationToken']?.value;
                    }
                }
                return _ajaxData;
            },
            processResults: (data, params) => {
                params.page = data.pageIndex + 1;
                return {
                    results: $.map(data.datas, (value, index) => {
                        return {
                            id: value.value,
                            text: value.text,
                            ...value
                        }
                    }), pagination: { more: ((data.pageIndex + 1) * 30) < data.totalCount }
                };
            }, cache: true
        },
        allowClear: true,
        escapeMarkup: (text) => { return text; },
        placeholder: t.attr('placeholder') || ''
    };
    if (otherParam) {
        Object.keys(otherParam).forEach(x => {
            m[x] = otherParam[x];
        });
    }
    t.select2(m);
};

const getModelFromForm = (form, isetup) => {
    const r = {};
    $.map(form, x => {
        if (x.name) {
            if ($(x).hasClass('mahas-currency') && isetup) {
                r[x.name] = isetup.cleave[x.name].getRawValue();
                return;
            } else if (x.nodeName === 'INPUT') {
                if (x.type === 'checkbox') {
                    r[x.name] = x.checked;
                    return;
                } else if (x.type === 'radio') {
                    if (x.checked) {
                        r[x.name] = x.value;
                    }
                    return;
                }
            }
            r[x.name] = x.value;
        }
    });
    $.map($(form).find('.table-detail'), i => {
        const s = [];
        const code = $(i).data('name');
        $.map($(i).find('tbody tr'), j => {
            const t = {};
            $.map($(j).data(), (k, l) => {
                if (l != 'model') {
                    t[l] = k;
                }
            });
            $.map($(j).find('[data-name],[name]'), k => {
                const name = $(k).data('name') || k.name;
                if (name) {
                    if ($(k).data('value')) {
                        t[name] = $(k).data('value');
                        return;
                    } else if (k.nodeName === 'INPUT') {
                        if (k.type === 'checkbox') {
                            t[name] = k.checked;
                            return;
                        } else if ($(k).hasClass('mahas-currency')) {
                            t[name] = isetup.cleave[`Detail_${code}_${name}_${JSON.stringify($(j).data('key'))}`].getRawValue();
                            return;
                        }
                    }
                    t[name] = k.value;
                }
            });
            s.push(t);
        });
        r[code] = s;
    });
    $.each($(form).find('.ipainter'), (i, v) => {
        if (validURL(v.src)) {
            //r[v.name] = await (new iPainter(v, { temp : true })).getSrc();
            r[v.name] = (new iPainter(v, { temp: true })).getSrc();
        } else {
            r[v.name] = v.src;
        }
    });
    return r;
};

const setFormFromModel = (form, model, isetup) => {
    $.map(model, (v, key) => {
        let input = form[key];
        if (!input) {
            for (var i = 1; i <= (key.length > 6 ? 6 : key.length); i++) {
                if (input) break;
                input = form[key.substr(0, i).toUpperCase() + key.slice(i)];
            }
        }
        if (!input) return;
        if ($(input).hasClass('mahas-currency') && isetup) {
            isetup.cleave[input.name].setRawValue(v);
            return;
        } else if ($(input).hasClass('ipainter') && isetup) {
            if (v) {
                $(input).attr('src', v.indexOf('data:image') === 0 ? v : `data:image/png;base64,${v}`);
            } else {
                $(input).attr('src', $(input).data('src'));
            }
            return;
        } else if (input.nodeName === 'SELECT' && $(input).hasClass('select2-hidden-accessible')) {
            if (v) {
                const _customText = $(input).data('text');
                $(input).append(`<option value="${v}">${model[_customText ? _customText : `${key}_Text`]}<option>`);
                if (model[`${key}_Object`]) {
                    $(input).find(`option[value="${v}"]`).data('object', model[`${key}_Object`]);
                }
                input.value = v;
                return;
            }
        } else if (input.nodeName === 'INPUT') {
            if (input.type === 'checkbox') {
                input.checked = v;
                return;
            } else if (input.type === 'date') {
                input.value = dateToString(v, 'yyyy-MM-dd');
                return;
            } else if (input.type === 'file') {
                return;
            }
        } else if (input.length > 1) {
            $.each(input, (i, v2) => {
                if (v2.type === 'radio' && v2.value === v) {
                    v2.checked = true;
                }
            });
        }
        if (v?.minutes) {
            input.value = v.hours + ':' + v.minutes;
        } else {
            input.value = v;
        }
    });
};

const removeTr = t => {
    const form = $(t).closest('form');
    $(t).closest('tr').remove();
    mahasFormulaProcessJs(form);
};

const isLoading = () => {
    return $('body').find('#isetup_loading').length > 0;
};

const loading = isLoading => {
    if (isLoading) {
        const html = `
                <div id="mahas_loading">
                    <i class="icon-spinner2 spinner"></i>
                </div>
            `;
        $('body').append(html);
    } else {
        $('body').find('#mahas_loading').remove();
    }
};

const objectToString = obj => {
    if (typeof obj === 'string' && jsonStringIsValid(obj)) {
        obj = JSON.parse(obj);
    }
    if (typeof obj === 'object') {
        return Object.keys(obj).map(key => Array.isArray(obj[key]) ? obj[key].join('\r') : obj[key]).join('\r');
    } else {
        return obj;
    }
};

const alertError = xhr => {
    let messate = xhr?.responseJSON?.title || xhr?.responseText || xhr?.statusText || 'Internal Server Error';
    messate = objectToString(messate);
    showAlert(messate, 'warning');
};

const validURL = str => {
    return str.indexOf('https://') >= 0 || str.indexOf('http://');
}

const alertWarning = message => {
    showAlert(message, 'warning');
};

const showAlert = (message, status) => {
    if ($('.mahas-alert').length === 0)
        $('body').append('<div class="mahas-alert"></div>');
    const time = new Date().getTime();
    let number = 0;
    let id = `mahas_alert_${time}_${number}`;
    while ($(`#${id}`).length) {
        number++;
        id = `mahas_alert_${time}_${number}`;
    }
    const html = `
        <div id="${id}" class="alert bg-${status || 'primary'} text-white alert-styled-left alert-dismissible">
            <button type="button" class="close" data-dismiss="alert"><span>×</span></button>
            ${message}
        </div>
    `;
    $('.mahas-alert').append(html);
    setTimeout(() => {
        $(`#${id}`).remove();
        if ($('.mahas-alert .alert').length === 0)
            $('.mahas-alert').remove();
    }, 20000);
};

const floatToCurrency = text => {
    if (text === '0') return '0';
    if (text === '') return '';
    if (text === '-0') return '-';
    if (!text) return '';
    text = text.toString().replace(/\,/g, '');
    const point = text.split('.')[1] || '';
    const number = parseFloat(text.toString().replace(/\./g, '.'));
    const arrResult = number.toString().replace(/\,/g, '.').split('.');
    let result = arrResult[0].substr(0, 16).replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    result = result === "NaN" ? "" : result;
    if (arrResult.length > 1) return result + '.' + point;
    else return result;
};

const currencyToFloat = currency => {
    if (!currency) { return ""; }
    if (currency === "-") currency = "-0";
    return parseFloat(currency.toString().replace(/[^0-9\.-]+/g, ""));
};

const dateToString = (v, format) => {
    if (!v) return '';
    //HH:mm:ss

    const date = new Date(v);
    if (isNaN(date)) return '';
    if (!format) format = 'yyyy-MM-dd';

    let chars = [];
    let _temp;
    let _chars;
    format.split('').forEach(x => {
        if (x === _temp) {
            _chars += x;
        } else {
            if (_chars) chars.push(_chars);
            _temp = x;
            _chars = _temp;
        }
    });
    if (_chars) chars.push(_chars);

    let tanggal = date.getDate();
    let bulan = date.getMonth() + 1;
    tanggal = tanggal < 10 ? '0' + tanggal : tanggal.toString();
    bulan = bulan < 10 ? '0' + bulan : bulan.toString();
    const tahun = date.getFullYear().toString();

    let jam = date.getHours();
    let menit = date.getMinutes();
    let detik = date.getSeconds();
    jam = jam < 10 ? '0' + jam : jam.toString();
    menit = menit < 10 ? '0' + menit : menit.toString();
    detik = jam < 10 ? '0' + detik : detik.toString();

    let r = '';
    chars.forEach(x => {
        if (x[0] === 'y') r += tahun.substr(4 - x.length);
        else if (x[0] === 'M') r += bulan.substr(2 - x.length);
        else if (x[0] === 'd') r += tanggal.substr(2 - x.length);
        else if (x[0] === 'H') r += jam.substr(2 - x.length);
        else if (x[0] === 'm') r += menit.substr(2 - x.length);
        else if (x[0] === 's') r += detik.substr(2 - x.length);
        else r += x;
    });
    return r;
};

const initDetailTr = (tr, model, code, isetup) => {
    $.each(tr.find(".mahas-currency"), (i, v) => {
        isetup.cleave[`Detail_${code}_${$(v).data('name')}_${JSON.stringify(tr.data('key'))}`] = new Cleave(v, { numeral: true, numeralThousandsGroupStyle: 'thousand' });
    });
    $.each(tr.find("select[data-datasource]"), (i, v) => {
        mahasSelect2($(v), $(v).data('datasource'), isetup.select2OtherParam);
    });
    Object.keys(model).forEach(x => {
        const y = $.map(tr.find(`[data-name]`), v => {
            if ($(v).data('name').toUpperCase() === x.toUpperCase()) return $(v);
        })[0];
        if (y) {
            switch (y.prop('tagName')) {
                case 'TD':
                    y.html(model[x]);
                    break;
                case 'INPUT':
                    if (y.attr('type') === 'checkbox')
                        y[0].checked = model[x];
                    else if (y.hasClass('mahas-currency'))
                        isetup.cleave[`Detail_${code}_${y.data('name')}_${JSON.stringify(tr.data('key'))}`].setRawValue(model[x]);
                    else if (y.attr('type') === 'date')
                        y.val(dateToString(model[x], 'yyyy-MM-dd'));
                    else
                        y.val(model[x]);
                    break;
                case 'SELECT':
                    if (y.data('datasource')) {
                        y.append(`<option value="${model[x]}">${model[x + `_Text`]}</option>`);
                    } else {
                        y.val(model[x]);
                    }
                    y.trigger('change');
                    break;
            }
        }
    });
    initHelper(tr, isetup);
};

const setCookie = (name, value, days) => {
    let expires = "";
    if (days) {
        let date = new Date();
        date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
};

const getCookie = name => {
    let nameEQ = name + "=";
    let ca = document.cookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
};

const clearCookie = name => {
    document.cookie = name + '=; Max-Age=-99999999;';
};

const initMahas = () => {
    if ($('.sidebar-main-toggle').length > 0) {
        $('.sidebar-main-toggle').on('click', () => {
            setCookie('sidebar', $('body').hasClass('sidebar-xs').toString(), 360);
        });
        let li = $('.sidebar.sidebar-main li a[href="' + window.location.pathname + '"]').parent('li');
        li.find('.nav-link').addClass('active');
        let parent = li.closest('.nav-item-submenu');
        if (parent.length > 0) {
            parent.find('a.nav-link').trigger('click');
        }
        if (getCookie('sidebar') === 'true') {
            $('body').addClass('sidebar-xs');
        }
    }
};

const modalDialog = (status, headerText, bodyText, callback) => {
    const html = `
        <div id="mahas_modal_dialog" class="modal fade" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header bg-${status || 'primary'}">
                        <h5 class="modal-title">${headerText || ''}</h5>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <p>${bodyText || ''}</p>
                    </div>
                    ${typeof callback === 'function' ?
            `
                    <div class="modal-footer">
                        <button type="button" data-dismiss="modal" class="btn btn-link">Cancel</button>
                        <button type="button" class="btn btn-${status || 'primary'} mahas-modal-dialog-yes">Yes</button>
                    </div>
                    ` : ''}
                </div>
            </div>
        </div>
    `;

    $('body').append(html);
    $('#mahas_modal_dialog').on('hidden.bs.modal', e => {
        $('#mahas_modal_dialog').remove();
    });
    if (typeof callback === 'function') {
        $('#mahas_modal_dialog .mahas-modal-dialog-yes').on('click', e => {
            callback();
            $('#mahas_modal_dialog .close').click();
        });
    }
    $('#mahas_modal_dialog').modal();
};

const passwordDialog = callback => {
    const html = `
        <div id="mahas_modal_password" class="modal fade" tabindex="-1">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header bg-primary">
                        <h5 class="modal-title">Please enter your password</h5>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <input type="password" class="form-control text-center"/>
                    </div>
                    <div class="modal-footer">
                        <button type="button" data-dismiss="modal" class="btn btn-link">Cancel</button>
                        <button type="button" class="btn btn-primary mahas-modal-password-yes">Yes</button>
                    </div>
                </div>
            </div>
        </div>
    `;

    $('body').append(html);
    $('#mahas_modal_password').on('hidden.bs.modal', e => {
        $('#mahas_modal_password').remove();
    });
    $('#mahas_modal_password .mahas-modal-password-yes').on('click', e => {
        if (typeof callback === 'function') callback($('#mahas_modal_password input[type="password"]').val());
        $('#mahas_modal_password .close').click();
    });
    $('#mahas_modal_password').modal();
};

const jsonStringIsValid = jsonString => {
    return /^[\],:{}\s]*$/.test(jsonString.replace(/\\["\\\/bfnrtu]/g, '@').
        replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']').
        replace(/(?:^|:|,)(?:\s*\[)+/g, ''));
};

const convertSpace = text => {
    return text
        .replace(/([a-z])([A-Z]|[0-9])/g, '$1 $2')
        .replace(/(ID|KODE)([A-Z])([a-z])/g, '$1 $2$3')
        .replace(/_/g, ' ')
        .replace(/\s\s+/g, ' ')
        .trim();
};

const detailSelectAllOnclick = t => {
    $(t).closest('form').find('tbody tr').addClass('detail-active');
};

const datediff = (first, second) => {
    return Math.round((second - first) / (1000 * 60 * 60 * 24));
}

const ipainterGetValue = t => {
    const v = $(t).attr('src');
    const rv = $(t).data('src');
    return v === rv ? null : v;
};

const idGenerator = () => {
    const S4 = () => {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    };
    return `${S4()}${S4()}${S4()}${S4()}${S4()}${S4()}`;
}

document.addEventListener("DOMContentLoaded", function (event) {
    initMahas();
});
