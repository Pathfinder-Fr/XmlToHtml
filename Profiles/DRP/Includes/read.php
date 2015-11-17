<?php
// Initialisation
$debug = isset($_GET['debug']) ? $_GET['debug'] : 0;
$filename = isset($_GET['file']) ? $_GET['file'] : '';

function showDebug($i, $filename)
{
  $exists = is_readable($filename) ? "200" : "404";
  echo "<blockquote>filename_$i=$filename ($exists)</blockquote>";
  foreach (str_split($filename) as $chr)
    $hex_ary[] = sprintf("%02X", ord($chr));
  echo "<pre>" . implode(' ',$hex_ary) . "</pre>";
}

function listFiles()
{
  echo "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\" /></head><body>";
  echo "<pre>" . shell_exec('ls -l') . "</pre>";
  echo "</body></html>";
}

function translateFileName($filename, $debug)
{
  if ($debug) echo "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /></head><body>";
  if ($debug) showDebug('raw', $filename);

  // Pre-Decodage
  //$filename = str_replace(array('Å“', 'œ'), array('æ', 'æ'), $filename);
  //if ($debug) showDebug('replaced', $filename);

  // Windows-1252 ou iso-8859-1
  $filename = mb_convert_encoding($filename, "Windows-1252", "utf-8") . '.html';

  if ($debug) showDebug('translated', $filename);

  // Post-decodage
  //$filename = str_replace(array('œ'), array('Å“'), $filename);
  //if ($debug) showDebug('translated_replaced', $filename);
  
  return $filename;
}

function readContentFile($filename, $debug)
{
  if (!is_readable($filename) || $filename[0] == '/' || $filename[0] == '.')
  {
    header('HTTP/1.0 404 Not Found');
  }
  else
  {
    $fp = fopen($filename, 'rb');
    while ($cline = fgets($fp))
    { 
      print $cline;
    }
    fclose($fp);
  }
}

if (isset($_GET['list']) && $_GET['list'] == "1")
{
  listFiles();
}
else
{
  $filename = translateFileName($filename, $debug);
  if (!$debug)
  {
    readContentFile($filename, $debug);
  }
  else
  {
    echo "</body></html>";
  }
}
?>