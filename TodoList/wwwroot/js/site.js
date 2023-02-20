const uri = 'api/TodoList';
let todos = []

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const taskTitleTextBox = document.getElementById('add-task');
    
    const todoItem = {
        isCompleted: false,
        task: taskTitleTextBox.value.trim()
    };
    
    fetch(uri, {
        method: 'post',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(todoItem)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            taskTitleTextBox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id){
    fetch(`${uri}/${id}`, {
        method: 'delete'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const todoItem = todos.find(item => item.id === id);

    document.getElementById('edit-task').value = todoItem.task;
    document.getElementById('edit-id').value = todoItem.id;
    document.getElementById('edit-isComplete').checked = todoItem.isCompleted;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isCompleted: document.getElementById('edit-isComplete').checked,
        task: document.getElementById('edit-task').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'put',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'task' : 'tasks';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('tasks');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isCompleteCheckbox = document.createElement('input');
        isCompleteCheckbox.type = 'checkbox';
        isCompleteCheckbox.classList.add("form-check-input");
        isCompleteCheckbox.disabled = true;
        isCompleteCheckbox.checked = item.isCompleted;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.classList.add("btn", "btn-primary");
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.classList.add("btn", "btn-primary");
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isCompleteCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.task);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    todos = data;
}