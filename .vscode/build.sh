#!/bin/bash

# Usage: build.sh [options] rules
# Options:
#   -d | --dir                  Used for default-values. Defaults to PWD.
#   -o | --out    | --outdir    Place the output into <file>
#                               Defaults to dir/bin
#   -s | --src    | --srcdir    Directory with the source-files.
#                               Defaults to dir/src
#   -r | --res    | --resdir    Directory with the resource-files.
#                               Defaults to dir/res
#   -n | --name                 The project name used for executable.
#                               Appended to outdir: "outdir/name".
#                               Defaults to "main"


parse_args() {
  # Loop over all arguments
  #for [ arg in "$@" ]; do # 'for arg; do' would be equivalent sugar
  while [ -n "$1" ]
  do
    local arg=$1
    
    # key-value pairs are prefixed by a '-'
    #local is_prefix_minus=["${arg:0:1}" = "-"] # Can't store result of boolean expressino in variable?
    local is_prefix_minus=false; [ "${arg:0:1}" == "-" ] && is_prefix_minus=true
    if [ "$is_prefix_minus" == true ]
    then
      # checks if argument contains '=',
      # pair is split over multiple args otherwise
      #local suffix=${$arg:1}
      #local is_pair=["$arg" = .*"=".*] # Can't store result of boolean expressino in variable?
      local is_pair=false; [ "$arg" == .*"=".* ] && is_pair=true

      # Get the key-value pair
      local key=""
      local val=""
      if [ "$is_pair" == true ]
      then
        key=${arg%%=*} # Gets the substring before '='
        val=${arg#*=} # Gets the substring after '='
      else
        # Get the value from next argument. How would I handle lists?
        key=$arg
        shift # Sort of like a look-ahead, while also removing form future iteration
        val="$1"
      fi

      case "$key" in
        # dir and output, or src and dst? Don't want conflict on the d.
        -d|--dir)
          dir=$val
          ;;
        -o|--out|--outdir)
          outdir=$val
          ;;
        -s|--src|--srcdir)
          srcdir=$val
          ;;
        -r|--res|--resdir)
          resdir=$val
          ;;
        -n|--name)
          name=$val
          ;;
        #-r|--run)
        #  run=true
        #  ;;
        --read)
          read=true
          ;;
      esac
    else
      # Positional argument, not a key-value pair.
      # Specifies which rule to use.
      rule=$arg
    fi

    shift # equivalent to removing first element of array

  done
}

assign_defaults() {
  PROJECTNAME=${name-"main"}
  DIR=${dir-$PWD}
  SRCDIR=${srcdir-"$DIR/$PROJECTNAME"}
  OUTDIR=${outdir-"$DIR/$PROJECTNAME/bin"}
  RESDIR=${resdir-"$DIR/$PROJECTNAME/Content"}
  BUILDDIR="$OUTDIR/Windows/x86/Debug/"
}

run_rule() {
  echo "rule=$rule"
  case "$rule" in
    run)
      run
      ;;
    *)
      default
  esac
}


# Parse arguments
#set -x
parse_args "$@" # Parse command line arguments
assign_defaults # Sets unset variables to their default-values.


# Pretend to be a makefile

build() {
  echo "build()"
  set -x
  # Two slashes needed because git-bash...
  # "E:/Program/Microsoft Visual Studio/2019/Community/Common7/IDE/devenv.exe" "$DIR/$PROJECTNAME.sln" //Build # Use devenv, seems to be more stable
  "E:/Program/Microsoft Visual Studio/2019/Community/MSBuild/Current/Bin/amd64/MSBuild.exe" "$DIR/$PROJECTNAME.sln" "-property:Configuration=Debug" #"-property:Platform=x64" # Use MSBuild, which sometimes seems to error on valuetuples special syntax
  set +x
}

default() {
  echo "default()"
  build
}

run() {
  echo "run()"
  build

  echo "run!"
  # Change working-dir, as executable must use its own location as root to find resources
  cd "$BUILDDIR"
  set -x
  "$BUILDDIR/$PROJECTNAME.exe"
  set +x
  cd $PWD
}


# run the selected rule
run_rule 

if  [ ${read-false} == true ]
then
  read # stay open once done
fi