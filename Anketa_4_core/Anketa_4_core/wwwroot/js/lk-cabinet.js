/* Shared LK_Data dashboard and modal editor for personal and moderator views.
 * Every update uses AJAX to replace only one section, preserving other JSON sections.
 */
(function () {
    "use strict";

    const field = (key, label, type = "text") => ({ key, label, type });
    const group = (key, label, fields) => ({ key, label, type: "list", fields });
    const statuses = field("Status", "Статус", "number");

    const taskFields = [
        field("Number", "№", "number"),
        field("ProfTaskName", "Название задачи"),
        field("DateFrom", "Начало", "date"),
        field("DateTo", "Окончание", "date"),
        field("AnalisEffect", "Анализ эффективности", "textarea"),
        field("Difficult", "Сложности", "textarea"),
        field("BossComment", "Комментарий руководителя", "textarea"),
        statuses
    ];
    const viceTaskFields = [
        field("TaskName", "Задача"),
        field("ReportAboutTask", "Отчёт о задаче", "textarea"),
        field("MarkAboutTask", "Оценка задачи", "textarea"),
        statuses
    ];

    const mapFields = [
        field("DateFrom", "Начало периода карты", "date"),
        field("DateTo", "Окончание периода карты", "date"),
        group("ProfessionalDevelopment", "Профессиональное развитие: комментарии к таблице 19", [
            field("CommentToT19", "Комментарий", "textarea"),
            field("RecomendationToT19", "Рекомендация", "textarea"),
            field("DialogResultToT19", "Результат беседы", "textarea")
        ]),
        group("ProfessionalDevelopment_T19", "Таблица 19 — Профессиональное развитие", [
            field("Header", "Заголовок"),
            field("Description", "Описание", "textarea")
        ]),
        group("ProfessionalDevelopment_T20", "Таблица 20 — Обучающие мероприятия", [
            field("Theme", "Тема мероприятия"),
            field("Format", "Формат"),
            field("Date", "Дата проведения", "date"),
            field("Interes", "Интерес", "textarea"),
            field("PracticalUse", "Практическая полезность", "textarea"),
            field("Quality", "Качество подачи", "textarea"),
            field("Comment", "Комментарий", "textarea"),
            field("Recomendation", "Рекомендация", "textarea"),
            field("Effect", "Что было эффективно", "textarea"),
            field("WhatBetter", "Что улучшить", "textarea")
        ]),
        group("ResultOrientation_T09", "Таблица 9 — Анализ деятельности", [
            field("TypeOfActivity", "Вид деятельности"),
            field("Processes", "Процессы", "textarea"),
            field("OrderDocument", "Регламентирующие документы", "textarea"),
            field("AdjustmentOfOrderDocument", "Корректировки документов", "textarea"),
            field("MissindOrderDocument", "Недостающие документы", "textarea"),
            field("BossRecomendation", "Рекомендации руководителя", "textarea")
        ]),
        group("ResultOrientation_T10", "Таблица 10 — План регламентирующих документов", [
            field("Type", "Процессы / старые документы"),
            field("NonOrderedProcess_OldDocument", "Нерегламентированные процессы", "textarea"),
            field("DevelopmentPlanOf_Order_Old_Document_Task", "План: задачи", "textarea"),
            field("DevelopmentPlanOfOrder_Old_Document_Date", "План: сроки"),
            field("MarkOfComplete", "Отметка о выполнении")
        ]),
        group("LK_Map_ResultOrientation_T11", "Таблица 11 — SMART и результаты", [
            field("Task", "Задача"),
            field("PlanResult_S_M_A_R_T", "Планируемый результат (SMART)", "textarea"),
            field("FactResult", "Фактический результат", "textarea"),
            field("DeviationOfFactResult", "Отклонения", "textarea"),
            field("DeviationReason", "Причины отклонений", "textarea"),
            field("MeasuresOfDeviationOfFactResult", "Меры предупреждения", "textarea"),
            field("FeedBack", "Обратная связь наставника", "textarea"),
            field("Conclusion", "Выводы", "textarea")
        ])
    ];

    const sections = [
        group("ProfTasks", "Профессиональные задачи", taskFields),
        group("Internships", "Стажировки", [
            field("DateFrom", "Начало", "date"),
            field("DateTo", "Окончание", "date"),
            statuses
        ]),
        group("LK_ViceBoss", "Замещения руководителя", [
            field("DateFrom", "Начало", "date"),
            field("DateTo", "Окончание", "date"),
            field("Target", "Цель замещения", "textarea"),
            group("ViceBoss_Tasks", "Задачи замещения", viceTaskFields)
        ]),
        group("ReportBooks", "Отчёты по прочитанным книгам", [
            field("BookName", "Название книги"),
            field("Author", "Автор"),
            field("Gold", "Ценность книги", "textarea"),
            field("PractiсeSkills", "Практические приёмы", "textarea"),
            statuses
        ]),
        group("TrainingProgramms", "Тренинговые программы", [
            field("LiveSityations", "Применённые приёмы и примеры", "textarea"),
            field("Difficult", "Сложности", "textarea"),
            field("InfluenceAsWorker", "Влияние на мою деятельность", "textarea"),
            field("InfluenceAsFilial", "Влияние на филиал", "textarea"),
            statuses
        ]),
        { key: "DevelopmentalActionMaproperty", label: "Карта развивающих действий",
          type: "object", fields: mapFields }
    ];

    const dom = (tag, classes, text) => {
        const e = document.createElement(tag);
        if (classes) e.className = classes;
        if (text !== undefined && text !== null) e.textContent = String(text);
        return e;
    };
    const btn = (label, classes, callback) => {
        const e = dom("button", "btn " + classes, label);
        e.type = "button";
        e.addEventListener("click", callback);
        return e;
    };
    const dateText = value => {
        const val = value == null ? "" : String(value);
        return val.startsWith("0001-01-01") ? "" : val.slice(0, 10);
    };
    function cellText(value, f) {
        if (f.type === "list") return (Array.isArray(value) ? value.length : 0) + " записей";
        if (f.type === "date") return dateText(value) || "—";
        if (value === undefined || value === null || value === "") return "—";
        return String(value);
    }
    const clone = x => JSON.parse(JSON.stringify(x));
    function emptyValue(f) {
        if (f.type === "list") return [];
        if (f.type === "object") return emptyRecord(f.fields);
        if (f.type === "number") return 0;
        return "";
    }
    function emptyRecord(fields) {
        const obj = {};
        fields.forEach(f => { obj[f.key] = emptyValue(f); });
        return obj;
    }
    function normalizeObject(fields, item) {
        const result = {};
        fields.forEach(f => {
            const val = item && item[f.key] !== undefined ? item[f.key] : emptyValue(f);
            if (f.type === "list") {
                result[f.key] = (Array.isArray(val) ? val : [])
                    .map(x => normalizeObject(f.fields, x));
            } else if (f.type === "date") {
                result[f.key] = dateText(val)
                    ? dateText(val) + "T00:00:00" : "0001-01-01T00:00:00";
            } else if (f.type === "number") {
                result[f.key] = Number.isFinite(Number(val)) ? Number(val) : 0;
            } else {
                result[f.key] = val == null ? "" : String(val);
            }
        });
        return result;
    }
    function normalizeSection(schema, val) {
        if (schema.type === "list")
            return (Array.isArray(val) ? val : []).map(x => normalizeObject(schema.fields, x));
        return normalizeObject(schema.fields, val || {});
    }
    function drawTable(parent, items, fields, title) {
        const container = dom("div", "mb-3");
        if (title) container.appendChild(dom("h6", "mt-3", title));
        if (!Array.isArray(items) || items.length === 0) {
            container.appendChild(dom("p", "small text-muted", "Нет записей."));
            parent.appendChild(container);
            return;
        }
        const wrap = dom("div", "table-responsive");
        const table = dom("table", "table table-sm table-striped table-bordered align-middle");
        const thead = dom("thead");
        const head = dom("tr");
        fields.forEach(f => head.appendChild(dom("th", "", f.label)));
        thead.appendChild(head);
        table.appendChild(thead);
        const tbody = dom("tbody");
        items.forEach((item, ix) => {
            const tr = dom("tr");
            fields.forEach(f => {
                const td = dom("td", "text-break");
                td.style.whiteSpace = "pre-line";
                td.textContent = cellText(item ? item[f.key] : null, f);
                tr.appendChild(td);
            });
            tbody.appendChild(tr);
            fields.filter(f => f.type === "list").forEach(f => {
                const detail = dom("tr");
                const td = dom("td");
                td.colSpan = fields.length;
                const panel = dom("details");
                const summary = dom("summary", "small fw-semibold", f.label + " — запись " + (ix + 1));
                panel.appendChild(summary);
                drawTable(panel, item && item[f.key], f.fields);
                td.appendChild(panel);
                detail.appendChild(td);
                tbody.appendChild(detail);
            });
        });
        table.appendChild(tbody);
        wrap.appendChild(table);
        container.appendChild(wrap);
        parent.appendChild(container);
    }
    function drawSection(parent, schema, value, edit) {
        const section = dom("section", "card shadow-sm mb-3");
        const head = dom("div", "card-header d-flex justify-content-between align-items-center gap-2");
        head.appendChild(dom("h5", "h6 mb-0", schema.label));
        head.appendChild(btn("Редактировать", "btn-outline-primary btn-sm", () => edit(schema)));
        section.appendChild(head);
        const body = dom("div", "card-body");
        if (schema.type === "list") {
            drawTable(body, value, schema.fields);
        } else {
            const map = value || {};
            const dates = dom("p", "small text-muted");
            dates.textContent = "Период: " + (dateText(map.DateFrom) || "—") +
                " — " + (dateText(map.DateTo) || "—");
            body.appendChild(dates);
            schema.fields.filter(f => f.type === "list")
                .forEach(f => drawTable(body, map[f.key], f.fields, f.label));
        }
        section.appendChild(body);
        parent.appendChild(section);
    }

    function renderFields(parent, record, fields, update) {
        const row = dom("div", "row g-3");
        fields.forEach(f => {
            if (f.type === "list") {
                const slot = dom("div", "col-12 border-top pt-3");
                slot.appendChild(dom("h6", "", f.label));
                if (!Array.isArray(record[f.key])) record[f.key] = [];
                renderArrayEditor(slot, record[f.key], f.fields, update);
                row.appendChild(slot);
                return;
            }
            const col = dom("div", f.type === "textarea" ? "col-12" : "col-md-6");
            const label = dom("label", "form-label", f.label);
            let control;
            if (f.type === "textarea") {
                control = dom("textarea", "form-control");
                control.rows = 3;
            } else {
                control = dom("input", "form-control");
                control.type = f.type === "date" ? "date"
                    : f.type === "number" ? "number" : "text";
                if (f.type === "number") control.step = "1";
            }
            control.value = f.type === "date" ? dateText(record[f.key])
                : (record[f.key] == null ? "" : String(record[f.key]));
            control.addEventListener("input", () => {
                record[f.key] = f.type === "number"
                    ? (parseInt(control.value, 10) || 0) : control.value;
                update(false);
            });
            label.appendChild(control);
            col.appendChild(label);
            row.appendChild(col);
        });
        parent.appendChild(row);
    }
    function renderArrayEditor(parent, arr, fields, update) {
        const stack = dom("div");
        if (arr.length === 0)
            stack.appendChild(dom("p", "text-muted small", "Записей пока нет."));
        arr.forEach((item, index) => {
            const card = dom("div", "card mb-3");
            const head = dom("div", "card-header d-flex justify-content-between");
            head.appendChild(dom("strong", "", "Запись " + (index + 1)));
            head.appendChild(btn("Удалить строку", "btn-outline-danger btn-sm", () => {
                arr.splice(index, 1);
                update(true);
            }));
            card.appendChild(head);
            const body = dom("div", "card-body");
            renderFields(body, item, fields, update);
            card.appendChild(body);
            stack.appendChild(card);
        });
        parent.appendChild(stack);
        parent.appendChild(btn("Добавить строку", "btn-outline-primary btn-sm mt-2", () => {
            arr.push(emptyRecord(fields));
            update(true);
        }));
    }

    async function init(root) {
        const compact = root.dataset.mode === "compact";
        const state = { data: {}, section: null, edited: null, busy: false };
        const modal = root.querySelector(".lk-edit-modal");
        const bsModal = window.bootstrap.Modal.getOrCreateInstance(modal);
        const area = root.querySelector(".lk-modal-body");
        const title = root.querySelector(".lk-modal-title");
        const saveButton = root.querySelector(".lk-save");
        const backButton = root.querySelector(".lk-back");
        const main = root.querySelector(".lk-main-sections");
        const mainError = root.querySelector(".lk-message");
        const modalError = root.querySelector(".lk-modal-error");

        function message(element, text) {
            element.textContent = text || "";
            element.hidden = !text;
        }

        function renderMain(target) {
            target.replaceChildren();
            sections.forEach(s => drawSection(target, s, state.data[s.key],
                schema => openEditor(schema)));
        }

        function showOverview() {
            state.section = null;
            title.textContent = "Данные личного кабинета";
            area.replaceChildren();
            renderMain(area);
            saveButton.hidden = true;
            backButton.hidden = true;
            message(modalError, "");
        }

        function renderEditor() {
            if (!state.section) return;
            area.replaceChildren();
            if (state.section.type === "list") {
                renderArrayEditor(area, state.edited, state.section.fields, rerender => {
                    if (rerender) renderEditor();
                });
            } else {
                renderFields(area, state.edited, state.section.fields, rerender => {
                    if (rerender) renderEditor();
                });
            }
        }

        function openEditor(schema) {
            state.section = schema;
            state.edited = clone(state.data[schema.key] || emptyValue(schema));
            title.textContent = schema.label;
            saveButton.hidden = false;
            backButton.hidden = !compact;
            message(modalError, "");
            renderEditor();
            if (!modal.classList.contains("show")) bsModal.show();
        }
        backButton.addEventListener("click", showOverview);
        if (compact) {
            root.querySelector(".lk-open-overview")
                .addEventListener("click", () => {
                    showOverview();
                    bsModal.show();
                });
        }
        saveButton.addEventListener("click", async () => {
            if (!state.section || state.busy) return;
            state.busy = true;
            saveButton.disabled = true;
            message(modalError, "");
            try {
                const antiForgery = root.querySelector('input[name="__RequestVerificationToken"]').value;
                const response = await fetch(root.dataset.saveUrl, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "RequestVerificationToken": antiForgery
                    },
                    credentials: "same-origin",
                    body: JSON.stringify({
                        section: state.section.key,
                        value: normalizeSection(state.section, state.edited)
                    })
                });
                const payload = await response.json();
                if (!response.ok)
                    throw new Error(payload.error || "Не удалось сохранить данные.");
                state.data = payload.lkData || {};
                if (compact) {
                    showOverview();
                } else {
                    renderMain(main);
                    bsModal.hide();
                }
                refreshRating();
            } catch (err) {
                message(modalError, err.message || "Ошибка сохранения.");
            } finally {
                state.busy = false;
                saveButton.disabled = false;
            }
        });

        async function refreshRating() {
            if (!root.dataset.ratingUrl) return;
            const slot = root.querySelector(".lk-ranking");
            try {
                const response = await fetch(root.dataset.ratingUrl, {
                    credentials: "same-origin", cache: "no-store"
                });
                if (!response.ok) throw new Error("Не удалось загрузить рейтинг.");
                const rating = await response.json();
                slot.replaceChildren();
                slot.appendChild(dom("h3", "h4 mb-3", "Рейтинг по периоду тестирования"));
                if (!rating.current) {
                    slot.appendChild(dom("p", "text-muted",
                        rating.formula || "Период тестирования пока не назначен."));
                    return;
                }
                slot.appendChild(dom("p", "mb-2",
                    "Период: " + rating.period + ". Участников: " + rating.participants +
                    ". Ваше место: " + rating.current.place + "."));
                for (const item of [
                    ["Заполненность таблиц", rating.current.tableCompletion],
                    ["Завершённость тестов", rating.current.testCompletion],
                    ["Общий рейтинг", rating.current.totalScore]
                ]) {
                    slot.appendChild(dom("div", "fw-semibold mt-3 mb-1",
                        item[0] + ": " + Number(item[1]).toFixed(1) + "%"));
                    const track = dom("div", "progress");
                    const bar = dom("div", "progress-bar");
                    bar.style.width = Math.min(100, Math.max(0, Number(item[1]))) + "%";
                    bar.setAttribute("role", "progressbar");
                    bar.setAttribute("aria-valuenow", Number(item[1]).toFixed(1));
                    bar.setAttribute("aria-valuemin", "0");
                    bar.setAttribute("aria-valuemax", "100");
                    track.appendChild(bar);
                    slot.appendChild(track);
                }
                slot.appendChild(dom("p", "text-muted small mt-2", rating.formula));
                slot.appendChild(dom("h4", "h5 mt-4", "Таблица рейтинга — первые 20 мест"));
                const wrap = dom("div", "table-responsive");
                const table = dom("table", "table table-sm table-striped");
                const thead = dom("thead");
                const header = dom("tr");
                ["Место", "Код", "Таблицы, %", "Тесты, %", "Результат, %"]
                    .forEach(x => header.appendChild(dom("th", "", x)));
                thead.appendChild(header);
                table.appendChild(thead);
                const tbody = dom("tbody");
                (rating.top || []).forEach(row => {
                    const tr = dom("tr", row.testableID === rating.current.testableID
                        ? "table-primary" : "");
                    [row.place, row.code, row.tableCompletion,
                        row.testCompletion, row.totalScore]
                        .forEach(value => tr.appendChild(dom("td", "", value)));
                    tbody.appendChild(tr);
                });
                table.appendChild(tbody);
                wrap.appendChild(table);
                slot.appendChild(wrap);
            } catch (err) {
                slot.textContent = err.message || "Рейтинг временно недоступен.";
            }
        }

        try {
            const response = await fetch(root.dataset.readUrl, {
                credentials: "same-origin", cache: "no-store"
            });
            const payload = await response.json();
            if (!response.ok) throw new Error(payload.error || "Не удалось загрузить данные.");
            state.data = payload.lkData || {};
            if (!compact) renderMain(main);
            message(mainError, "");
        } catch (err) {
            message(mainError, err.message || "Ошибка загрузки.");
            if (compact) root.querySelector(".lk-open-overview").disabled = true;
            return;
        }
        refreshRating();
    }

    document.addEventListener("DOMContentLoaded", function () {
        document.querySelectorAll(".lk-dashboard").forEach(init);
    });
})();
