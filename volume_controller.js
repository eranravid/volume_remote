const http = require('http');
const exec = require('child_process').exec;
const hostname = '10.0.0.1';
const port = 8006;
var volSteps = 1911;


const server = http.createServer((req, res) => {

	if (req.method === "GET") {

		req.params = params(req); // call the function above ;
		if (!req.params.method || !req.params.vol) {
			res.end(`Please use the querystring, define methos = (set/add) and vol = (volume value)`);
			return;
		}
		console.log(`Requested ${req.params.method} methos with volume value of ${req.params.vol}`);

		volSteps = req.params.vol;
		volSteps = (volSteps / 100) * 65535; // get a number between 0-100 as nircmd max volume is 65535

		// choose api method
		var nircmdMethod = "";
		if (req.params.method === "set") {
			nircmdMethod = "setsysvolume";
		} else if (req.params.method === "add") {
			nircmdMethod = "changesysvolume";
		} else {
			res.end(`Please choose method set or add`);
			return;
		}

		exec(`nircmd.exe ${nircmdMethod} ${volSteps}`, (error, stdout, stderr) => {
			if (error) {
				console.error(`exec error: ${error}`);
				return;
			}

		});
	}

	res.statusCode = 200;
	res.setHeader('Content-Type', 'text/html');
	res.end(renderHTML());

});

function addVolume() {
	server.request("/?method=add&vol=5");
}

function renderHTML() {
	return (`
		<button onClick="addVolume()">add + 5</button>
		<button onClick="reduceVolume()">add - 5</button>
		
		`);
}

server.listen(port, hostname, () => {
	console.log(`Server running at http://${hostname}:${port}/`);
});

var params = function(req) {
	let q = req.url.split('?'),
		result = {};
	if (q.length >= 2) {
		q[1].split('&').forEach((item) => {
			try {
				result[item.split('=')[0]] = item.split('=')[1];
			} catch (e) {
				result[item.split('=')[0]] = '';
			}
		})
	}
	return result;
}