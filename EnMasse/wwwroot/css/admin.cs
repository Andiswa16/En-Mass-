.admin - layout {
display: flex;
}

/* Sidebar */
.sidebar - container {
width: 260px;
    min - height: 100vh;
background: #1f2937;
}

.sidebar - container a {
    display: block;
color: white;
margin: 10px 0;
text - decoration: none;
}

/* Main area */
.main - content {
    flex - grow: 1;
padding: 24px;
background: #f3f4f6;
}

/* Top bar */
.top - bar {
display: flex;
    justify - content: space - between;
    margin - bottom: 20px;
}

/* Stats cards */
.stats - grid {
display: grid;
    grid - template - columns: repeat(4, 1fr);
gap: 15px;
}

.stats - card {
background: white;
padding: 20px;
    border - radius: 12px;
    box - shadow: 0 2px 10px rgba(0,0,0,0.08);
    text - align: center;
}

/* Tables */
.table - style {
width: 100 %;
background: white;
    border - collapse: collapse;
}

.table - style th, .table-style td {
    padding: 10px;
border: 1px solid #ddd;
}

/* Buttons */
.btn - danger {
background: red;
color: white;
border: none;
padding: 5px 10px;
}