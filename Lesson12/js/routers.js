async function loadStudents() {
    const table = document.querySelector('#studentsTable');
    const tbody = table.querySelector('tbody');
    const errorP = document.querySelector('#error');

    tbody.innerHTML = '';
    errorP.textContent = '';

    const res = await fetch('/students/all');
    if (!res.ok) {
        table.style.display = 'none';
        errorP.textContent = 'Ошибка: HTTP ' + res.status;
        return;
    }

    const students = await res.json();

    for (const s of students) {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>${s.studentId}</td>
            <td>${s.firstName}</td>
            <td>${s.lastName}</td>
            <td>${s.age}</td>`;
        tbody.appendChild(tr);
    }

    table.style.display = 'table';
}

async function loadGroups() {
    const table = document.querySelector('#groupsTable');
    const tbody = table.querySelector('tbody');
    const errorP = document.querySelector('#errorGroups');

    tbody.innerHTML = '';
    errorP.textContent = '';

    const res = await fetch('/groups/all');
    if (!res.ok) {
        table.style.display = 'none';
        errorP.textContent = 'Ошибка: HTTP ' + res.status;
        return;
    }

    const groups = await res.json();

    for (const g of groups) {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>${g.groupId}</td>
            <td>${g.groupName}</td>`;
        tbody.appendChild(tr);
    }

    table.style.display = 'table';
}