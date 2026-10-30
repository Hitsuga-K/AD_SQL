const configs = {
    students: {
        fields: ["studentId", "firstName", "lastName", "age", "groupId"],
        formFields: ["firstName", "lastName", "age", "groupId"],
        labels: ["ID", "Имя", "Фамилия", "Возраст", "ID группы"],
        idField: "studentId",
        table: "studentsTable",
        error: "studentsError",
        emptyText: "Студентов пока нет."
    },
    groups: {
        fields: ["groupId", "groupName"],
        formFields: ["groupName"],
        labels: ["ID", "Название группы"],
        idField: "groupId",
        table: "groupsTable",
        error: "groupsError",
        emptyText: "Групп пока нет."
    },
    subjects: {
        fields: ["id", "subjectName", "teacherId"],
        formFields: ["subjectName", "teacherId"],
        labels: ["ID", "Предмет", "ID преподавателя"],
        idField: "id",
        table: "subjectsTable",
        error: "subjectsError",
        emptyText: "Предметов пока нет."
    },
    teachers: {
        fields: ["id", "firstName", "lastName", "age", "subjectId"],
        formFields: ["firstName", "lastName", "age", "subjectId"],
        labels: ["ID", "Имя", "Фамилия", "Возраст", "ID предмета"],
        idField: "id",
        table: "teachersTable",
        error: "teachersError",
        emptyText: "Преподавателей пока нет."
    },
    grades: {
        fields: ["id", "studentId", "subjectId", "teacherId", "gradeValue", "gradeDate"],
        formFields: ["studentId", "subjectId", "teacherId", "gradeValue", "gradeDate"],
        labels: ["ID", "ID студента", "ID предмета", "ID преподавателя", "Оценка", "Дата"],
        idField: "id",
        table: "gradesTable",
        error: "gradesError",
        emptyText: "Оценок пока нет."
    }
};

function getConfig(type) {
    const config = configs[type];

    if (!config) {
        throw new Error("Неизвестный раздел: " + type);
    }

    return config;
}

function getItemId(type, item) {
    return item[getConfig(type).idField] ?? item.id;
}

function formatCellValue(value) {
    if (value == null) return "";

    if (typeof value === "string" && value.includes("T")) {
        const date = new Date(value);

        if (!Number.isNaN(date.getTime())) {
            return date.toLocaleDateString("ru-RU");
        }
    }

    return String(value);
}

async function requestJson(url, options = {}) {
    const response = await fetch(url, {
        ...options,
        headers: {
            ...(options.body
                ? { "Content-Type": "application/json" }
                : {}),
            ...options.headers
        }
    });

    if (!response.ok) {
        let details = "";

        try {
            details = await response.text();
        } catch (_) { }

        throw new Error(
            `HTTP ${response.status}${details ? ": " + details : ""}`
        );
    }

    if (response.status === 204) {
        return null;
    }

    const text = await response.text();

    return text ? JSON.parse(text) : null;
}


async function loadData(type) {
    const config = getConfig(type);

    const table = document.getElementById(config.table);
    const error = document.getElementById(config.error);

    if (!table) {
        console.error(`Таблица #${config.table} не найдена в index.html`);
        return;
    }

    const tbody = table.querySelector("tbody");

    if (!tbody) return;

    tbody.replaceChildren();

    if (error) {
        error.textContent = "Загрузка...";
    }

    try {
        const items = await requestJson(`/${type}/all`);

        if (!Array.isArray(items)) {
            throw new Error("Сервер вернул данные не в формате массива.");
        }

        for (const item of items) {
            const row = document.createElement("tr");

            for (const field of config.fields) {
                const cell = document.createElement("td");

                cell.textContent = formatCellValue(item[field]);

                row.appendChild(cell);
            }

            const actions = document.createElement("td");

            const editButton = document.createElement("button");
            editButton.type = "button";
            editButton.textContent = "Изменить";

            editButton.addEventListener("click", () => {
                editItem(type, item);
            });

            const deleteButton = document.createElement("button");
            deleteButton.type = "button";
            deleteButton.textContent = "Удалить";
            deleteButton.classList.add("delete-button");

            deleteButton.addEventListener("click", () => {
                deleteItem(type, item);
            });

            actions.append(editButton, deleteButton);

            row.appendChild(actions);
            tbody.appendChild(row);
        }

        if (error) {
            error.textContent = items.length === 0
                ? config.emptyText
                : "";
        }

    } catch (exception) {
        if (error) {
            error.textContent =
                `Не удалось загрузить данные: ${exception.message}`;
        }

        console.error(`Ошибка загрузки раздела ${type}:`, exception);
    }

    updateStatistics();
}


async function getItem(type, id) {
    return requestJson(`/${type}/${encodeURIComponent(id)}`);
}


function getFormId(form) {
    const visibleId = form.querySelector(
        'input[name="id"]:not([type="hidden"])'
    );

    const hiddenId = form.querySelector(
        'input[type="hidden"][name="id"]'
    );

    return (visibleId?.value || hiddenId?.value || "").trim();
}

function setFormId(form, id) {
    form.querySelectorAll('[name="id"]').forEach(input => {
        input.value = id ?? "";
    });
}


async function saveItem(type, form) {
    const config = getConfig(type);

    const formData = new FormData(form);
    const item = {};

    for (const field of config.formFields) {
        const rawValue = formData.get(field);

        const input = form.querySelector(`[name="${field}"]`);

        if (rawValue === null) continue;

        if (input && input.type === "number") {
            item[field] = rawValue === ""
                ? null
                : Number(rawValue);
        } else {
            item[field] = String(rawValue).trim();
        }
    }

    const id = getFormId(form);

    const isEdit = id !== "";

    const url = isEdit
        ? `/${type}/${encodeURIComponent(id)}`
        : `/${type}`;

    const method = isEdit ? "PUT" : "POST";

    try {
        await requestJson(url, {
            method: method,
            body: JSON.stringify(item)
        });

        clearForm(form.id);

        await loadData(type);

        alert(
            isEdit
                ? "Запись успешно изменена."
                : "Запись успешно добавлена."
        );

    } catch (exception) {
        const error = document.getElementById(config.error);

        if (error) {
            error.textContent =
                `Не удалось сохранить запись: ${exception.message}`;
        }

        console.error(exception);

        alert(`Ошибка сохранения: ${exception.message}`);
    }
}


async function deleteItem(type, item) {
    const id = getItemId(type, item);

    if (id == null) {
        alert("Не удалось определить ID записи.");
        return;
    }

    if (!confirm(`Удалить запись №${id}?`)) {
        return;
    }

    try {
        await requestJson(
            `/${type}/${encodeURIComponent(id)}`,
            {
                method: "DELETE"
            }
        );

        await loadData(type);

    } catch (exception) {
        const error = document.getElementById(
            getConfig(type).error
        );

        if (error) {
            error.textContent =
                `Не удалось удалить запись: ${exception.message}`;
        }

        console.error(exception);

        alert(`Ошибка удаления: ${exception.message}`);
    }
}

function editItem(type, item) {
    const form = document.querySelector(
        `form[data-type="${type}"]`
    );

    if (!form) return;

    setFormId(form, getItemId(type, item));

    for (const field of getConfig(type).formFields) {
        const input = form.querySelector(`[name="${field}"]`);

        if (input) {
            let value = item[field] ?? "";

            if (input.type === "date" && value) {
                value = String(value).slice(0, 10);
            }

            input.value = value;
        }
    }

    form.scrollIntoView({
        behavior: "smooth",
        block: "center"
    });
}

function clearForm(formId) {
    const form = document.getElementById(formId);

    if (!form) return;

    form.reset();

    setFormId(form, "");
}

function filterTable(input, tableId) {
    const query = input.value.toLocaleLowerCase("ru");

    const rows = document.querySelectorAll(
        `#${tableId} tbody tr`
    );

    rows.forEach(row => {
        row.style.display =
            row.textContent.toLocaleLowerCase("ru").includes(query)
                ? ""
                : "none";
    });
}

function updateStatistics() {
    for (const type of Object.keys(configs)) {
        const table = document.getElementById(configs[type].table);

        if (!table) continue;

        const count = table.querySelectorAll("tbody tr").length;

        const total = document.getElementById(`${type}Total`);

        if (total) {
            total.textContent = count;
        }

        const statIds = {
            students: "studentCount",
            groups: "groupCount",
            subjects: "subjectCount",
            teachers: "teacherCount",
            grades: "gradeCount"
        };

        const stat = document.getElementById(statIds[type]);

        if (stat) {
            stat.textContent = count;
        }
    }
}


async function loadStudents() {
    await loadData("students");
}

async function loadGroups() {
    await loadData("groups");
}

async function loadSubjects() {
    await loadData("subjects");
}

async function loadTeachers() {
    await loadData("teachers");
}

async function loadGrades() {
    await loadData("grades");
}


document.addEventListener("DOMContentLoaded", () => {

    for (const type of Object.keys(configs)) {
        const form = document.querySelector(
            `form[data-type="${type}"]`
        );

        if (form) {
            form.addEventListener("submit", event => {
                event.preventDefault();

                saveItem(type, form);
            });
        }
    }

    for (const type of Object.keys(configs)) {
        loadData(type);
    }

    updateStatistics();
});