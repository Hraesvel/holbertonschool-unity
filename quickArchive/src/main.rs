use std::fs;
use std::fs::DirEntry;
use std::path::{Path, PathBuf};
use structopt::StructOpt;

fn main() {
	let ops = Opts::from_args();
	let project_name = ops.project_name.clone();

	let ent = Path::new(ops.root_dir.as_str())
		.read_dir().unwrap()
		.into_iter()
		.map(|mut x|
			{
				if let Some(f) = x.ok() {
					if !&f.file_type().unwrap().is_dir() {
						None
					} else {
						if f.path().read_dir().unwrap().next().is_none() ||
							f.path() == Path::new("../quickArchive").to_path_buf()
							{
							None
						} else {
							Some(f.path())
						}
					}
				} else {
					None
				}
			})
		.into_iter()
		.filter(|x| x.is_some())
		.collect::<Vec<Option<PathBuf>>>();

	for entry in ent {

		let fname = entry.as_ref().unwrap().file_name().unwrap();
		let p = entry.as_ref().unwrap();

		let file_name: String = if let Some(n) = fname.to_str() {
			if n == "Mac" {
				format!("{pname}_Mac", pname = &project_name)
			} else {
				format!("{pname}_{name}_x86_64", pname = &project_name, name = n)
			}
		} else {
			panic!("failure")
		};

		let fs = fs::OpenOptions::new()
			.write(true)
			.truncate(true)
			.create(true)
			.open(format!("{}/{}.zip", &ops.output_dir ,file_name))
			.unwrap();

		let op = zip::write::FileOptions::default()
			.compression_method(zip::CompressionMethod::Deflated);
		zip_dir::zip_dir(&p, fs, Some(op));
	}
}

#[derive(Debug, StructOpt)]
struct Opts {
	#[structopt( name = "root directory", short = "r", long = "root" , default_value = ".")]
	root_dir : String,

	#[structopt(name = "output directory", short = "o", default_value = ".")]
	output_dir : String,

	#[structopt(name = "ignore empty", short = "i")]
	ignore_empty : bool,

	#[structopt(name = "Project Name", short = "p")]
	project_name: String,
}
