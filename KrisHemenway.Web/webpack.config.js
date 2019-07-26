const CleanWebpackPlugin = require("clean-webpack-plugin");
const CopyPlugin = require("copy-webpack-plugin");

module.exports = {
	entry: {
		app: "./src/Home/App.tsx",
		calendar: "./src/Calendar/Calendar.tsx"
	},

	output: {
		filename: "[name].js",
		path: __dirname + "/dist"
	},

	devtool: "source-map",

	resolve: {
		extensions: [".ts", ".tsx", ".js", ".json"],
	},

	module: {
		rules: [
			{ test: /\.(png|jpg|gif)$/, use: [{ loader: "file-loader", options: {} }], },
			{ test: /\.tsx?$/, loader: "ts-loader" },
			{ enforce: "pre", test: /\.js$/, loader: "source-map-loader" },
		]
	},

	plugins: [
		new CleanWebpackPlugin(["./build", "./dist"]),
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