const apiBaseUrl = ''; // Base URL (e.g., /api/eventflow) if needed, otherwise leave as an empty string

// Fetch and display all events
async function viewAllEvents() {
    try {
        const response = await fetch(`${apiBaseUrl}/GetAllEvents`);
        if (!response.ok) throw new Error('Failed to fetch events.');

        const data = await response.json();
        const events = data.Events || []; // Adjust structure if backend response differs

        let contentHtml = '<h2>All Events</h2><ul>';
        if (events.length === 0) {
            contentHtml += '<p>No events found.</p>';
        } else {
            events.forEach(event => {
                contentHtml += `
                    <li>
                        <strong>${event.Title}</strong> - ${event.EventLocation}
                        <p>${event.EventDescription}</p>
                        <p>Total Attendees: ${event.TotalAttendees}</p>
                    </li>`;
            });
        }
        contentHtml += '</ul>';

        document.getElementById('content').innerHTML = contentHtml;
    } catch (error) {
        console.error('Error fetching events:', error);
        document.getElementById('content').innerHTML = '<p>Error fetching events. Please try again later.</p>';
    }
}

// Display the Create Reservation form
function createReservation() {
    const formHtml = `
        <h2>Create a Reservation</h2>
        <form onsubmit="submitReservation(event)">
            <div>
                <label>Reservation Time</label>
                <input type="time" id="reservationTime" required />
            </div>
            <div>
                <label>Reservation Date</label>
                <input type="date" id="reservationDate" required />
            </div>
            <div>
                <label>User ID</label>
                <input type="number" id="userId" required />
            </div>
            <div>
                <label>Event ID</label>
                <input type="number" id="eventId" required />
            </div>
            <button type="submit">Create Reservation</button>
        </form>
    `;
    document.getElementById('content').innerHTML = formHtml;
}

// Submit a new reservation
async function submitReservation(event) {
    event.preventDefault();

    const reservation = {
        ReservationTime: document.getElementById('reservationTime').value,
        ReservationDate: document.getElementById('reservationDate').value,
        Status: 'Pending',
        UserId: parseInt(document.getElementById('userId').value, 10),
        EventId: parseInt(document.getElementById('eventId').value, 10)
    };

    try {
        const response = await fetch(`${apiBaseUrl}/AddReservation`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(reservation)
        });

        if (response.ok) {
            alert('Reservation created successfully!');
            document.getElementById('content').innerHTML = '';
        } else {
            const error = await response.json();
            alert(`Failed to create reservation: ${error.message}`);
        }
    } catch (error) {
        console.error('Error creating reservation:', error);
        alert('Error creating reservation. Please try again later.');
    }
}

// Fetch and display reservations for a user
async function viewMyReservations() {
    const userId = prompt('Enter your User ID:');
    if (!userId) return;

    try {
        const response = await fetch(`${apiBaseUrl}/GetReservations/${userId}`);
        if (!response.ok) throw new Error('Failed to fetch reservations.');

        const data = await response.json();
        const reservations = data.Reservations || []; // Adjust structure based on backend response

        let contentHtml = '<h2>My Reservations</h2><ul>';
        if (reservations.length === 0) {
            contentHtml += '<p>No reservations found.</p>';
        } else {
            reservations.forEach((reservation, index) => {
                contentHtml += `
                    <li>
                        ${index + 1}. 
                        <strong>Time:</strong> ${reservation.ReservationTime}, 
                        <strong>Date:</strong> ${reservation.ReservationDate}, 
                        <strong>Status:</strong> ${reservation.Status}
                    </li>`;
            });
        }
        contentHtml += '</ul>';

        document.getElementById('content').innerHTML = contentHtml;
    } catch (error) {
        console.error('Error fetching reservations:', error);
        document.getElementById('content').innerHTML = '<p>Error fetching reservations. Please try again later.</p>';
    }
}

// Fetch and display events created by the user
async function viewMyEvents() {
    const userId = prompt('Enter your User ID:');
    if (!userId) return;

    try {
        const response = await fetch(`${apiBaseUrl}/GetEventsByUser/${userId}`);
        if (!response.ok) throw new Error('Failed to fetch events.');

        const data = await response.json();
        const events = data.Events || []; // Adjust structure based on backend response

        let contentHtml = '<h2>My Events</h2><ul>';
        if (events.length === 0) {
            contentHtml += '<p>No events found.</p>';
        } else {
            events.forEach((event, index) => {
                contentHtml += `
                    <li>
                        ${index + 1}. 
                        <strong>Title:</strong> ${event.Title}, 
                        <strong>Location:</strong> ${event.EventLocation}, 
                        <strong>Description:</strong> ${event.EventDescription}, 
                        <strong>Total Attendees:</strong> ${event.TotalAttendees}
                    </li>`;
            });
        }
        contentHtml += '</ul>';

        document.getElementById('content').innerHTML = contentHtml;
    } catch (error) {
        console.error('Error fetching events:', error);
        document.getElementById('content').innerHTML = '<p>Error fetching events. Please try again later.</p>';
    }
}

// Display the Create User form
function createUser() {
    const formHtml = `
        <h2>Create User</h2>
        <form onsubmit="submitUser(event)">
            <div>
                <label>Username</label>
                <input type="text" id="username" required />
            </div>
            <div>
                <label>Email</label>
                <input type="email" id="email" required />
            </div>
            <button type="submit">Create User</button>
        </form>
    `;
    document.getElementById('content').innerHTML = formHtml;
}

// Submit a new user
async function submitUser(event) {
    event.preventDefault();

    const user = {
        Username: document.getElementById('username').value,
        Email: document.getElementById('email').value
    };

    try {
        const response = await fetch(`${apiBaseUrl}/CreateUser`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(user)
        });

        if (response.ok) {
            alert('User created successfully!');
            document.getElementById('content').innerHTML = '';
        } else {
            const error = await response.json();
            alert(`Failed to create user: ${error.message}`);
        }
    } catch (error) {
        console.error('Error creating user:', error);
        alert('Error creating user. Please try again later.');
    }
}

// Display the Delete Reservation form
function deleteReservation() {
    const formHtml = `
        <h2>Delete Reservation</h2>
        <form onsubmit="submitDeleteReservation(event)">
            <div>
                <label>Reservation ID</label>
                <input type="number" id="reservationId" required />
            </div>
            <button type="submit">Delete Reservation</button>
        </form>
    `;
    document.getElementById('content').innerHTML = formHtml;
}

// Submit delete reservation request
async function submitDeleteReservation(event) {
    event.preventDefault();

    const reservationId = document.getElementById('reservationId').value;

    try {
        const response = await fetch(`${apiBaseUrl}/DeleteReservation/${reservationId}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            alert('Reservation deleted successfully!');
            document.getElementById('content').innerHTML = '';
        } else {
            const error = await response.json();
            alert(`Failed to delete reservation: ${error.message}`);
        }
    } catch (error) {
        console.error('Error deleting reservation:', error);
        alert('Error deleting reservation. Please try again later.');
    }
}

// Display the Update Reservation form
function updateReservation() {
    const formHtml = `
        <h2>Update Reservation</h2>
        <form onsubmit="submitUpdateReservation(event)">
            <div>
                <label>Reservation ID</label>
                <input type="number" id="updateReservationId" required />
            </div>
            <div>
                <label>Reservation Time</label>
                <input type="time" id="updateReservationTime" required />
            </div>
            <div>
                <label>Reservation Date</label>
                <input type="date" id="updateReservationDate" required />
            </div>
            <div>
                <label>Status</label>
                <input type="text" id="updateStatus" required />
            </div>
            <button type="submit">Update Reservation</button>
        </form>
    `;
    document.getElementById('content').innerHTML = formHtml;
}

// Submit update reservation request
async function submitUpdateReservation(event) {
    event.preventDefault();

    const reservationId = document.getElementById('updateReservationId').value;
    const reservation = {
        ReservationTime: document.getElementById('updateReservationTime').value,
        ReservationDate: document.getElementById('updateReservationDate').value,
        Status: document.getElementById('updateStatus').value
    };

    try {
        const response = await fetch(`${apiBaseUrl}/UpdateReservation/${reservationId}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(reservation)
        });

        if (response.ok) {
            alert('Reservation updated successfully!');
            document.getElementById('content').innerHTML = '';
        } else {
            const error = await response.json();
            alert(`Failed to update reservation: ${error.message}`);
        }
    } catch (error) {
        console.error('Error updating reservation:', error);
        alert('Error updating reservation. Please try again later.');
    }
}
