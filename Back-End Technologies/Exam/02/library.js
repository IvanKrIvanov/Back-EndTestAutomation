function library(input) {
    const n = parseInt(input[0]);
    let books = input.slice(1, n + 1);

    const commands = input.slice(n + 1);
    for (let command of commands) {
        const tokens = command.split(' ');
        const action = tokens[0];

        if (action === 'Stop') {
            if (books.length > 0) {
                console.log("Books left: " + books.join(", "));
            } else {
                console.log("The library is empty");
            }
            break;
        } else if (action === 'Lend') {
            if (books.length > 0) {
                console.log(`${books.shift()} book lent!`);
            }
        } else if (action === 'Return') {
            const bookTitle = tokens.slice(1).join(' ');
            books.unshift(bookTitle);
        } else if (action === 'Exchange') {
            const startIndex = parseInt(tokens[1]);
            const endIndex = parseInt(tokens[2]);
            if (startIndex < 0 || startIndex >= books.length || endIndex < 0 || endIndex >= books.length) {
                continue;
            }
            [books[startIndex], books[endIndex]] = [books[endIndex], books[startIndex]];
            console.log('Exchanged!');
        }
    }
}

