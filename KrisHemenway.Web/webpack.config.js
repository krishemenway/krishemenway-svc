const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const CopyPlugin = require("copy-webpack-plugin");
const path = require("path");

module.exports = {
	entry: {
		app: "./src/Home/App.tsx",
		missingepisodes: "./src/MissingEpisodes/MissingEpisodes.tsx",
		calendar: "./src/Calendar/Calendar.tsx"
	},

	output: {
		filename: "[name].js",
		path: __dirname + "/dist"
	},

	resolve: {
		extensions: [".ts", ".tsx", ".js", ".json"],
		modules: [
			path.resolve(__dirname, "src"),
			"node_modules"
		]
	},

	module: {
		rules: [
			{ test: /\.(png|jpg|gif)$/, use: [{ loader: "file-loader", options: {} }], },
			{ test: /\.tsx?$/, loader: "ts-loader" },
		]
	},

	plugins: [
		new CleanWebpackPlugin({ cleanStaleWebpackAssets: false }),
		new CopyPlugin([
			{ from: "./src/favicon.ico", to: ".", flatten: false },
			{ from: "./src/**/*.html", to: ".", flatten: true },
			{ from: "./misc_projects", to: ".", flatten: false },
		]),
	],

	externals: {
		"jquery": "$",
		"react": "React",
		"react-dom": "ReactDOM",
		"moment": "moment",
	},
};